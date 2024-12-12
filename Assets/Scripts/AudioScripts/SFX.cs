using Game;
using UnityEngine;

public static class SFX
{
   
   public static void ATK(Entity.EntityType entityType, Vector3 position) => PlaySfx(FXType.Attack, entityType, position);
   public static void DMG(Entity.EntityType entityType, Vector3 position) => PlaySfx(FXType.Damage, entityType, position);
   public static void DEATH(Entity.EntityType entityType, Vector3 position) => PlaySfx(FXType.Death, entityType, position);
   public static void MOVE(Entity.EntityType entityType, Vector3 position) => PlaySfx(FXType.Move, entityType, position);


   private static void PlaySfx(FXType fxType, Entity.EntityType entityType, Vector3 position)
   {
       var fmodManager = FMODManager.instance;
       string eventName = null;
 switch (fxType)
    {
        case FXType.Attack:
            switch (entityType)
            {
                case Entity.EntityType.HumanSpearman: eventName = "ATK_Human.Spear"; break;
                case Entity.EntityType.HumanArcher: eventName = "ATK_Human.Archer"; break;
                case Entity.EntityType.DemonSwordsman: eventName = "ATK_Demon.Sword"; break;
                case Entity.EntityType.DemonTank: eventName = "ATK_Demon.Tank"; break;
            }
            break;

        case FXType.Damage:
            switch (entityType)
            {
                case Entity.EntityType.HumanSpearman: eventName = "DMG_Human.Spear"; break;
                case Entity.EntityType.HumanArcher: eventName = "DMG_Human.Archer"; break;
                case Entity.EntityType.DemonSwordsman: eventName = "DMG_Demon.Sword"; break;
                case Entity.EntityType.DemonTank: eventName = "DMG_Demon.Tank"; break;
            }
            break;

        case FXType.Death:
            switch (entityType)
            {
                case Entity.EntityType.HumanSpearman: eventName = "DEATH_Human.Spear"; break;
                case Entity.EntityType.HumanArcher: eventName = "DEATH_Human.Archer"; break;
                case Entity.EntityType.DemonSwordsman: eventName = "DEATH_Demon.Sword"; break;
                case Entity.EntityType.DemonTank: eventName = "DEATH_Demon.Tank"; break;
            }
            break;

        case FXType.Move:
            switch (entityType)
            {
                case Entity.EntityType.HumanSpearman: eventName = "MOVE_Human.Spear"; break;
                case Entity.EntityType.HumanArcher: eventName = "MOVE_Human.Archer"; break;
                case Entity.EntityType.DemonSwordsman: eventName = "MOVE_Demon.Sword"; break;
                case Entity.EntityType.DemonTank: eventName = "MOVE_Demon.Tank"; break;
            }
            break;
    }
       

    if (eventName != null)
    {
        fmodManager.OneShot(eventName, position);
    }
    else
    {
        Debug.Log("Event name not found.");
    }
   }
   }

   public enum FXType
   {
      Attack,
      Damage,
      Death,
      Move
      
      
   }

   // omg allt detta jobbet bara så jag slipper skriva två extra ord varje gång pog!

   
         
         
         
         



