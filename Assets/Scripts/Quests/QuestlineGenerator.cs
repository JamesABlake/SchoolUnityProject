using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class QuestlineGenerator {
	static List<Type> types = new() { typeof(Quest_CollectMinerals), typeof(Quest_GoToLocation), typeof(Quest_DestroyTargets) };
	public static Questline GenerateRandomQuestline(int length) {
		var questline = ScriptableObject.CreateInstance<Questline>();
		for(int i = 0; i < length; i++) {
			Quest quest = (Quest)ScriptableObject.CreateInstance(Helpers.Pick(types));
			quest.GenerateRandom();

			if(quest is Quest_DestroyTargets destroyQuest) {
				if(Random.Range(0, 100) < 50) {
					var prevQuest = ScriptableObject.CreateInstance<Quest_GoToLocation>();
					prevQuest.GenerateRandom();
					prevQuest.DesiredLocation = destroyQuest.SpawnInfo[0].TargetPosition;
					questline.AddQuest(prevQuest);
				}

				questline.AddQuest(quest);

				if(Random.Range(0, 100) < 50) {
					Quest followupQuest = ScriptableObject.CreateInstance<Quest_ReturnHome>();
					followupQuest.GenerateRandom();
					questline.AddQuest(followupQuest);
				}
				continue;
			}

			questline.AddQuest(quest);

			if(quest is Quest_GoToLocation) {
				if(Random.Range(0, 100) < 50) {
					Quest followupQuest = ScriptableObject.CreateInstance<Quest_ReturnHome>();
					followupQuest.GenerateRandom();
					questline.AddQuest(followupQuest);
				}

			}
		}
		return questline;
	}
}
