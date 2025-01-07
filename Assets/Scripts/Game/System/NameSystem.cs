using System.Collections.Generic;
using System.IO;
using Game;
using UnityEngine;

public class NameSystem : MonoBehaviour
{
    private List<Identity> used = new List<Identity>();
    private List<string> currentlyUsed = new List<string>();
    private List<string[]> humanInfo;
    private List<string[]> demonInfo;
    private StreamReader reader;
    // Start is called before the first frame update
    void Start()
    {
        if (FindFirstObjectByType<NameSystem>() != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RefillNames(TextAsset human, TextAsset demons)
    {
        humanInfo = new List<string[]>();
        demonInfo = new List<string[]>();

        string[] info = human.text.Split('\n');
        if (info.Length % 4 != 0) Debug.Log("Human file has incorrect amount of lines");
        for (int i = 0; i < info.Length; i += 4)
        {
            humanInfo.Add(new[] { info[i], info[i + 1], info[i + 2], info[i + 3] });
        }

        info = demons.text.Split('\n');
        if (info.Length % 2 != 0) Debug.Log("Demon file has incorrect amount of lines");
        for (int i = 0; i < info.Length; i += 2)
        {
            demonInfo.Add(new[] { info[i], info[i + 1] });
        }
    }

    public bool NoNames()
    {
        return humanInfo == null;
    }

    public void GiveIdentity(Entity e)
    {
        if (e.isHuman)
        {
            foreach (Identity identity in used)
            {
                if (e.Type == identity.type)
                {
                    if (!currentlyUsed.Contains(identity.name))
                    {
                        e.AssignIdentity(new[] { identity.name, identity.gender, identity.age, identity.description });
                        currentlyUsed.Add(identity.name);
                        return;
                    }
                }

            }

            e.AssignIdentity(GenerateIdentity(true));
            used.Add(new Identity()
            {
                type = e.Type, gender = e.IsMale ? "M" : "F", name = e.Name, age = e.Age,
                description = e.Description
            });
            currentlyUsed.Add(e.Name);

        }
        else
        {
            e.AssignIdentity(GenerateIdentity(false));
        }
    }

    private string[] GenerateIdentity(bool isHuman)
    {
        if (isHuman)
        {
            string[] namn = humanInfo[Random.Range(1, humanInfo.Count)];
            humanInfo.Remove(namn);
            return namn;
        }
        else
        {
            string[] namn = demonInfo[Random.Range(1, demonInfo.Count)];
            namn = new[] { namn[0], "D", "", namn[1] };
            demonInfo.Remove(namn);
            return namn;
        }
    }

    public void NewLevel()
    {
        currentlyUsed.Clear();
    }
}


public class Identity
{
    public Entity.EntityType type;
    public string name;
    public string gender;
    public string age;
    public string description;
}