using System;
using Game;
using UnityEngine;

public class SpecialAbilities : MonoBehaviour
{
    public static void CheckForAbility(Entity.EntityType entityType, int abilityNumber)
    {
        switch (entityType)
        {
            case Entity.EntityType.HumanSpearman:
                HumanSpearmanCheckAbility(abilityNumber); 
                break;
            case Entity.EntityType.HumanArcher:
                HumanArcherCheckAbility(abilityNumber);
                break;
            case Entity.EntityType.HumanTank:
                HumanTankCheckAbility(abilityNumber);
                break;
            case Entity.EntityType.DemonSwordsman:
                DemonSwordsmanCheckAbility(abilityNumber);
                break;
            case Entity.EntityType.DemonTank:
                DemonTankCheckAbility(abilityNumber);
                break;
            case Entity.EntityType.DemonArcher:
                DemonArcherCheckAbility(abilityNumber);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
        }
    }

    private static void HumanSpearmanCheckAbility(int abilityNumber)
    {
        switch (abilityNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityNumber), abilityNumber, null);
        }
    }

    private static void HumanArcherCheckAbility(int abilityNumber)
    {
        switch (abilityNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityNumber), abilityNumber, null);
        }
    }

    private static void HumanTankCheckAbility(int abilityNumber)
    {
        switch (abilityNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityNumber), abilityNumber, null);
        }
    }

    private static void DemonSwordsmanCheckAbility(int abilityNumber)
    {
        switch (abilityNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityNumber), abilityNumber, null);
        }
    }

    private static void DemonTankCheckAbility(int abilityNumber)
    {
        switch (abilityNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityNumber), abilityNumber, null);
        }
    }

    private static void DemonArcherCheckAbility(int abilityNumber)
    {
        switch (abilityNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityNumber), abilityNumber, null);
        }
    }
}

