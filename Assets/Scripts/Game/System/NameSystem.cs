using System.Collections.Generic;
using System.IO;
using Game;
using UnityEngine;

public class NameSystem : MonoBehaviour
{
    private List<Identity> used = new List<Identity>();
    private List<string> currentlyUsed = new List<string>();
    public List<string> dead = new List<string>();
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
        used = new List<Identity>();
        currentlyUsed = new List<string>();
        dead = new List<string>();
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
                    if (!currentlyUsed.Contains(identity.name) && !dead.Contains(identity.name))
                    {
                        e.AssignIdentity(identity);
                        used.Add(identity);
                        currentlyUsed.Add(identity.name);
                        return;
                    }
                }

            }

            Identity id = GenerateIdentity(true);
            e.AssignIdentity(id);
            used.Add(id);
            currentlyUsed.Add(e.Name);

        }
        else
        {
            Identity id;
            do id = GenerateIdentity(false);
            while (currentlyUsed.Contains(id.name));
            e.AssignIdentity(id);
            currentlyUsed.Add(id.name);
        }
    }

    private Identity GenerateIdentity(bool isHuman)
    {
        if (isHuman)
        {
            string[] namn = humanInfo[Random.Range(1, humanInfo.Count)];
            humanInfo.Remove(namn);
            Identity id = new Identity()
            {
                name = namn[0],
                isMale = namn[1][0] == 'M',
                age = namn[2],
                description = namn[3],
                face = GetFace(namn[0], true, namn[1][0] == 'M')
            };
            return id;
        }
        else
        {
            string[] namn = demonInfo[Random.Range(1, demonInfo.Count)];
            demonInfo.Remove(namn);
            Identity id = new Identity()
            {
                name = namn[0],
                isMale = false,
                age = "",
                description = namn[1],
                face = GetFace(namn[0], false, false)
            };
            return id;
        }
    }

    private Sprite GetFace(string namn, bool isHuman, bool isMale)
    {
        if (isHuman)
        {
            Texture2D tex = Resources.Load<Texture2D>($"CharacterPortrait/{(isMale?"M":"F")}_Human{namn.Split(' ')[0]}");
            if (tex != null)
                return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
            Debug.LogError($"COULD NOT FIND HUMAN FACE ICON {(isMale?"M":"F")}_Human{namn.Split(' ')[0]}");
        }
        else
        {
            //DEMON ICON HERE :3
        }

        return null;
    }

    public void Kill(string namn)
    {
        dead.Add(namn);
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
    public bool isMale;
    public string age;
    public string description;
    public Sprite face;
}