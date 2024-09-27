using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProceduralTerrain : MonoBehaviour
{

	[Header("타일맵 관련")]
	[SerializeField] private Terrain terrain;
	[Space]
	[Header("값 관련")]
	//노이즈의 스케일 값이 클수록 새밀한 노이즈가 표현된다
	[SerializeField] private float mapScale = 0.003f;
	//맵의 사이즈
	[SerializeField] private int mapSize = 4096;
	//맵의 최대높이
	[SerializeField] private int depth = 600;

	//랜덤 생성을위한 시드
	private float seed;

	private async void Start()
	{

		seed = Random.Range(0, 10000f);

		var noiseArr = await Task.Run(GenerateNoise);

		SetTerrain(noiseArr);

	}

	//실질적으로 지형을 설정하는 함수
	private void SetTerrain(float[,] noiseArr)
	{

		//크기를 설정
		terrain.terrainData.size = new Vector3(mapSize, depth, mapSize);

		//지형 그리기
		terrain.terrainData.SetHeights(0, 0, noiseArr);

	}

	//노이즈 생성 함수
	private float[,] GenerateNoise()
	{

		float[,] noiseArr = new float[mapSize, mapSize];

		for (int x = 0; x < mapSize; x++)
		{

			for (int y = 0; y < mapSize; y++)
			{

				//랜덤한 노이즈를 생성
				noiseArr[x, y] = Mathf.PerlinNoise(
					x * mapScale + seed,
					y * mapScale + seed);

			}

		}

		return noiseArr;

	}


}
