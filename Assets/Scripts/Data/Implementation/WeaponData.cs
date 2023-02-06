using System;
using General.Enums;

namespace Implementation.Data
{
	/// <summary>
	/// Holds implementation of <see cref="IWeaponData"/>.
	/// </summary>
	[Serializable]
	public class WeaponData : IWeaponData
	{
		/// <inheritdoc />
		public string Id { get; set; }

		/// <inheritdoc />
		public Die DieToRoll { get { return dieToRoll; } set { dieToRoll = value; } }
		public Die dieToRoll;

		/// <inheritdoc />
		public WeaponType Type { get { return type; } set { type = value; } }
		public WeaponType type;
	}
}
