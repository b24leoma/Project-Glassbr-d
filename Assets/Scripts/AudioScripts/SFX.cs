using System.Collections.Generic;
using Game;
using UnityEngine;

public static class SFX
{
    private static readonly Dictionary<(FXType, string, Entity.EntityType), string> HumanEventNameCache = new();
    private static readonly Dictionary<(FXType, Entity.EntityType), string> DemonEventNameCache = new();
    public static void ATK(Entity entity) => PlaySfx(FXType.ATK, entity);
    public static void DMG(Entity entity) => PlaySfx(FXType.DMG, entity);
    public static void DEATH(Entity entity) => PlaySfx(FXType.DEATH, entity);
    public static void MOVE(Entity entity) => PlaySfx(FXType.MOVE, entity);

    private static void PlaySfx(FXType fxType, Entity entity)
    {
        var fmodManager = FMODManager.instance;

        var eventName = GetEventName(fxType, entity);

        if (eventName == null)
        {
            Debug.Log("Event name not found."); 
            return;
        }
        
        fmodManager.OneShot(eventName, entity.transform.position);
        
    }

    private static string GetEventName(FXType fxType, Entity entity)
    {
        if (entity.isHuman)   //För Humans
        {
            var key = (fxType, GetHumanInfo(entity), entity.Type);
            if (HumanEventNameCache.TryGetValue(key, out var name))
            {
                return name;
            }

            var eventName = $"{fxType}_{GetHumanInfo(entity)}_{entity.Type}";
            HumanEventNameCache[key] = eventName;
            return eventName;
        }
        else    //För Demons
        {
            var key = (fxType, entity.Type);
            if (DemonEventNameCache.TryGetValue(key, out var name))
            {
                return name;
            }

            var eventName = $"{fxType}_{entity.Type}";
            DemonEventNameCache[key] = eventName;
            return eventName;
        }
    }

    private static string GetHumanInfo(Entity entity)
    {
        var boolConverter = "";

        if (entity.IsMale)
        {
            boolConverter += "M";
        }
        else
        {
            boolConverter += "F";
        }

        if (entity.isOld)
        {
            boolConverter += "_Old";
        }
        else
        {
            boolConverter += "_Young";
        }

        return boolConverter;
    }
}

public enum FXType
{
    ATK,
    DMG,
    DEATH,
    MOVE
}

// omg allt detta jobbet bara så jag slipper skriva två extra ord varje gång pog!