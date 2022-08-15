using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class MobManager : MonoBehaviour
{
    public GameObject[] Mobs;
    public MobWave[] Waves;
    public List<DropTable> dropTables;

    public Events.EventIntegerEvent OnMobKilled;
    public Events.EventIntegerEvent OnWaveCompleted;
    public UnityEvent OnOutOfWaves;

    public int currentWaveIndex = 0;
    public int activeMobs;

    private SpawnPoint[] spawnPoints;
    public Transform[] wayPoints;
    void Start()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();
        SpawnWave();
    }

    public void SpawnWave()
    {
        if(Waves.Length -1 < currentWaveIndex)
        {
            OnOutOfWaves.Invoke();
           // Debug.LogWarning("No waves Left. You Win!");
            return;
        }

        if(currentWaveIndex > 0)
        {
            SoundManager.Instance.PlaySoundEffect(SoundEffects.NextWave);
        }

        activeMobs = Waves[currentWaveIndex].NumberOfMobs;
        for(int i = 0; i <= Waves[currentWaveIndex].NumberOfMobs - 1; i++)
        {
            SpawnPoint sp = selectedSpawnPoint();
            GameObject mob = Instantiate(selectedRandomMob(), sp.transform.position, Quaternion.identity);

            mob.GetComponent<NPCController>().waypoints = wayPoints;//findClosestWayPoints(mob.transform);

            CharacterStats stats = mob.GetComponent<CharacterStats>();
            MobWave currentWave = Waves[currentWaveIndex];

            stats.SetInitialHealth(currentWave.MobHealth);
            stats.SetInitialResistance(currentWave.MobResistance);
            stats.SetInitialDamage(currentWave.MobDamage);
        }
    }

    public void OnMobDeth(MobType mobtype, Vector3 pos)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffects.MobDeath);
        SpawnDrop(mobtype, pos);

        MobWave currentwave = Waves[currentWaveIndex];

        activeMobs -= 1;
        Debug.LogWarningFormat("Mob died {1} remaining",mobtype, pos);
        OnMobKilled.Invoke(currentwave.PointsPerKill);

        if(activeMobs == 0)
        {
            OnWaveCompleted.Invoke(currentwave.WaveValue);
            currentWaveIndex += 1;
            SpawnWave();
        }
    }
    private GameObject selectedRandomMob()
    {
        int mobIndex = Random.Range(0, Mobs.Length);
        return Mobs[mobIndex];
    }
    private SpawnPoint selectedSpawnPoint()
    {
        int pointIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[pointIndex];
    }
    private Transform[] findClosestWayPoints(Transform mobTransform)
    {
        Vector3 mobPosition = mobTransform.position;

        WayPoint closestPoint = FindObjectsOfType<WayPoint>().OrderBy(
            w => (w.transform.position - mobPosition).sqrMagnitude).First();

        Transform parent = closestPoint.transform.parent;

        Transform[] allTransforms = parent.GetComponentsInChildren<Transform>();

        var transform = from t in allTransforms where t != parent select t;

        return transform.ToArray();
    }
    private void SpawnDrop(MobType type, Vector3 pos)
    {
        ItemPickUp_SO item = getDrop(type);

        if(item != null)
        {
            Instantiate(item.itemSpawnObject, pos, Quaternion.identity);
        }
    }
    private ItemPickUp_SO getDrop(MobType mob)
    {
        DropTable mobDrops = dropTables.Find(mt => mt.mobType == mob);

        if(mobDrops == null)
        {
            return null;
        }

        mobDrops.drops.OrderBy(d => d.DropChance);

        foreach(DropDefinition dropDef in mobDrops.drops)
        {
            bool shouldDrop = Random.value < dropDef.DropChance;
            if (shouldDrop)
            {
                return dropDef.Drop;
            }
        }
        return null;
    }
}
