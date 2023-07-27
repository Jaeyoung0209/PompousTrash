using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class, new()
{
    static Singleton<T> instance;

    public static T Instance
    {
        get
        {
            return instance as T;
        }
    }

    protected virtual void Awake()
    {
        instance = this;

    }
}

public class GameManager : Singleton<GameManager>
{
    private PlayerControl Player;
    public GameObject NpcPrefab;
    [SerializeField]
    private List<NPC> Npc;
    public List<GameObject> MissionPrefab;
    public Camera maincamera;
    public int NpcNumber = 5;
    [SerializeField]
    private GameObject Ongoingmission = null;
    private NPC missionnpc = null;
    [SerializeField]
    private GameObject ReButton;

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerControl>();
        InstantiateNpc();
        ReButton.SetActive(false);
    }
    private void InstantiateNpc()
    {
        for (int i = 0; i < NpcNumber; i++)
        {
            Vector2 npclocation = new Vector2(Random.Range(-140, 140), Random.Range(-140, 140));
            while (true)
            {
                if (Vector2.Distance(npclocation, Vector2.zero) > 10f)
                    break;
                npclocation = new Vector2(Random.Range(-140, 140), Random.Range(-140, 140));
            }
            GameObject Newnpc = Instantiate(NpcPrefab, npclocation, Quaternion.identity);
            Npc.Add(Newnpc.GetComponent<NPC>());
        }
    }
    void DestroyAll(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }
    public void MissionEnd()
    {
        Ongoingmission = null;
        missionnpc.resolved();
        missionnpc = null;
        DestroyAll("mission");
        Player.controlable = true;
        Player.heal(4);
        for (int i = 0; i < Npc.Count; i++)
        {
            Npc[i].moveable = true;
        }

    }

    public void MissionStart(int mission, NPC npc)
    {
        if (Ongoingmission == null && missionnpc == null)
        {
            missionnpc = npc;
            Player.controlable = false;
            for (int i = 0; i < Npc.Count; i++)
            {
                Npc[i].moveable = false;
            }

            if (mission == 1)
            {
                Ongoingmission = Instantiate(MissionPrefab[0], maincamera.transform.position, Quaternion.identity);
            }
            else if (mission == 2)
            {
                Ongoingmission = Instantiate(MissionPrefab[1], maincamera.transform.position, Quaternion.identity);
            }
        }
    }
    
    public void PlayerDead()
    {
        if(Ongoingmission != null)
        {
            Ongoingmission = null;
            missionnpc = null;
        }
        DestroyAll("mission");
        Player.controlable = false;
        for (int i = 0; i < Npc.Count; i++)
        {
            Npc[i].moveable = false;
        }
        ReButton.SetActive(true);
    }

    public void Restart()
    {
        Npc = new List<NPC>();
        DestroyAll("npc");
        Player.transform.position = new Vector2(-10, 0);
        Player.life = 100;
        InstantiateNpc();
        ReButton.SetActive(false);
        Player.controlable = true;
    }
}
