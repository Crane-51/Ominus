using System.Linq;
using Character.Stats;
using General.State;
using UnityEngine;
using Implementation.Data;

namespace Player.Mechanic.Combat
{
    public class RightPunch : PlayerCombatState
    {
		//[InjectDiContainter]
		private IWeaponData weaponData { get; set; }

		protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 16;
			weaponData = SaveAndLoadData<IWeaponData>.LoadSpecificData(GetComponent<CharacterStatsMono>().Weapon.ToString());
		}

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            var enemies = physicsOverlap.Box(transform, rangeOfAttack).Where(x => x.GetComponent<CharacterTakeDamage>() != null && !(x.GetComponent<StateController>().ActiveHighPriorityState is CharacterIsDead)).Select(x => x.GetComponent<CharacterTakeDamage>()).ToList();
            
            //Get closest enemy.
            var target = enemies.FirstOrDefault(x => Vector2.Distance(x.transform.position, transform.position) == enemies.Min(y => Vector2.Distance(y.transform.position, transform.position)));

            if(target != null)
            {
				bonusDmg = target.transform.localScale.x == transform.localScale.x ? pcc.SneakBonusToApply : 0;
				int dmg = DiceRoller.RollDieWithModifier(weaponData.DieToRoll, 
					weaponData.Type == General.Enums.WeaponType.Melee ? GetComponent<CharacterStatsMono>().Strength : GetComponent<CharacterStatsMono>().Dexterity)
					+ bonusDmg;
				target.TakeDamage(dmg, transform.localScale.x, 1, true);
				GetComponentInChildren<SoundControllerHelper>().PlaySound(targetHitEvent);
			}

            StartCoroutine(AttackColdown());
        }
    }
}
