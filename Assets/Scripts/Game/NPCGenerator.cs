﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Vis;


public class NPCGenerator : MonoBehaviour, IResetable, IPausable
{
	public int WaveIndex { get { return waveIndex; }}

	public Action AllWavesFinishedAction;
	public Action<int, int>  WaveFinishedAction;
	public Action<int, int>  NextWaveAction;
	public Action<int> NPCLeftChagedAction;
	public Action<Character> CharacterGenerated;
	public AnimationCurve spawnCurve;
	public bool Running { get { return running; } set { running = value; }}
	public class Context
	{
		public LevelFrame LevelFrame;
		public InGameCamera GameCamera;
	}

	[SerializeField]
	WaveSettings waves;

	[SerializeField]
	List<GameObject> NpcPrefabs;

	float generateTime = 0;
	float startTime;
	float timer;

	int generatedNPCCount;
	int killedNPCs;
	int waveIndex;
	bool running;
	bool paused;


	Context context;

	public void Init(Context context)
	{
		this.context = context;
		running = true;
		NextWaveAction(waveIndex + 1, waves.EnemiesInWave.Count);
		NPCLeftChagedAction(GetEnemiesLeft());
	}


	#region IResetable implementation
	public void Reset ()
	{
		AllWavesFinishedAction = null;
		NextWaveAction = null;
		NPCLeftChagedAction = null;
		WaveFinishedAction = null;
		CharacterGenerated = null;
		waveIndex = 0;
		generateTime = 0;
		killedNPCs = 0;
		generatedNPCCount = 0;
		startTime = 0;
	}
	#endregion


	public void OnNPCDeath()
	{
		bool wavesFinished = false;

		killedNPCs++;
		if (killedNPCs == waves.EnemiesInWave[waveIndex]) {
			WaveFinishedAction(waveIndex, waves.EnemiesInWave.Count);
			killedNPCs = 0;
			generatedNPCCount = 0;
			waveIndex = Math.Min(waveIndex + 1, waves.EnemiesInWave.Count);
			NextWaveAction(waveIndex + 1, waves.EnemiesInWave.Count);
			wavesFinished = waveIndex == waves.EnemiesInWave.Count;
		}
		if (wavesFinished) {
			running = false;
			AllWavesFinishedAction();
		} else  {
			NPCLeftChagedAction(GetEnemiesLeft());
		}
	}

	void Update()
	{
		if (running) {
			if (timer > generateTime) {
				GenerateRandomNPC();
				float delay = GetSpawnDelay(Time.time - startTime);
				generateTime = timer + delay;
			}	
		}

		if (!paused) {
			timer += Time.deltaTime;
		}
	}

	void GenerateRandomNPC()
	{
		if (generatedNPCCount < waves.EnemiesInWave[waveIndex]) {
			var prefab = NpcPrefabs[UnityEngine.Random.Range(0, NpcPrefabs.Count)];
			var npc = Instantiate(prefab);

			npc.transform.position = GetRandomPositionOutsideScreen();

			if (CharacterGenerated != null) {
				CharacterGenerated(npc.GetComponent<Character>());
			}

			generatedNPCCount++;
		}
	}

	Vector3 GetRandomPositionOutsideScreen()
	{
		float camWidh = context.GameCamera.GetWidth();

		float rndY = UnityEngine.Random.Range(
			context.LevelFrame.GetMinY(),
			context.LevelFrame.GetMaxY());
		float rndX = context.GameCamera.GetPosition().x + camWidh * (Utils.GetRandomBool() ? -1 : 1);
		return new Vector3(rndX, rndY, 0);
	}

	float GetSpawnDelay(float elapsedTime)
	{
		return spawnCurve.Evaluate(Time.time - startTime);
	}

	int GetEnemiesLeft()
	{
		return waves.EnemiesInWave[waveIndex] - killedNPCs;
	}

	public void Pause ()
	{
		paused = true;
	}

	public void Resume ()
	{
		paused = false;
	}
}

