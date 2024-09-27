using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProceduralTerrain : MonoBehaviour
{

	[Header("Ÿ�ϸ� ����")]
	[SerializeField] private Terrain terrain;
	[Space]
	[Header("�� ����")]
	//�������� ������ ���� Ŭ���� ������ ����� ǥ���ȴ�
	[SerializeField] private float mapScale = 0.003f;
	//���� ������
	[SerializeField] private int mapSize = 4096;
	//���� �ִ����
	[SerializeField] private int depth = 600;

	//���� ���������� �õ�
	private float seed;

	private async void Start()
	{

		seed = Random.Range(0, 10000f);

		var noiseArr = await Task.Run(GenerateNoise);

		SetTerrain(noiseArr);

	}

	//���������� ������ �����ϴ� �Լ�
	private void SetTerrain(float[,] noiseArr)
	{

		//ũ�⸦ ����
		terrain.terrainData.size = new Vector3(mapSize, depth, mapSize);

		//���� �׸���
		terrain.terrainData.SetHeights(0, 0, noiseArr);

	}

	//������ ���� �Լ�
	private float[,] GenerateNoise()
	{

		float[,] noiseArr = new float[mapSize, mapSize];

		for (int x = 0; x < mapSize; x++)
		{

			for (int y = 0; y < mapSize; y++)
			{

				//������ ����� ����
				noiseArr[x, y] = Mathf.PerlinNoise(
					x * mapScale + seed,
					y * mapScale + seed);

			}

		}

		return noiseArr;

	}


}
