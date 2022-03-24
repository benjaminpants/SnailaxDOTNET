using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SnailaxDOTNET
{

	public struct SquidProperties
	{
		public float GroundSpikeProbability;
		public float WallSpikeProbablity;
		public float CeilingSpikeProbability;
		public float AirSpikeProbability;
		public float BulletProbablity;
		public bool CompletelyDisabled;
		public float IceSpikeFallProbability;
		public float FireworksProbability;
		public float ConveyerBeltChangeProbability;
		public float LaserProbability;

		public SquidProperties(float spp, float wsp, float csp, float asp, float bp, bool cd, float isfp, float cbcp, float lp, float fw)
		{
			GroundSpikeProbability = spp;
			WallSpikeProbablity = wsp;
			CeilingSpikeProbability = csp;
			AirSpikeProbability = asp;
			BulletProbablity = bp;
			CompletelyDisabled = cd;
			IceSpikeFallProbability = isfp;
			ConveyerBeltChangeProbability = cbcp;
			LaserProbability = lp;
			FireworksProbability = fw;
		}
	}

	public enum Themes
	{
		Default,
		Bubblegum,
		Disco,
		Underwater,
		Brain,
		Winter
	}

	public enum Songs
	{
		JumpAndDie,
		SimulatedLife,
		SimulatedLifeUnderwater,
		QuietlySearching,
		MakeItPain,
		AdmittingDefeat,
		ShellyFire,
		DemolitionWarning,
		DiscoOfDoom,
		ChillZone,
		MrDance,
		Underwater,
		MamaSquid,
		DeathByNanobot,
		HelpyLovesYou,
		WinterMode,
		ArtificialJoy,
		Tension,
		FinalEncounter,
		RealityDiving,
		BrainAmbience,
		ShellyFireUnderwater

	}
}
