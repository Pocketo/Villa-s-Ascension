using UnityEngine;

public class TerrainRandomTexture : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float rockAngleThreshold = 30f;  // Ángulo desde donde aparece roca
    [SerializeField] private float blendRange         = 10f;  // Zona de mezcla entre texturas

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();

        if (terrain == null)
        {
            Debug.LogError("No se encontró componente Terrain.");
            return;
        }

        ApplyTextures(terrain.terrainData);
    }

    private void ApplyTextures(TerrainData terrainData)
    {
        int mapWidth  = terrainData.alphamapWidth;
        int mapHeight = terrainData.alphamapHeight;
        int layers    = terrainData.alphamapLayers;

        if (layers < 2)
        {
            Debug.LogError("El terreno necesita al menos 2 capas de textura en Terrain Layers.");
            return;
        }

        // Unity usa [y, x, capa] — no [x, y, capa]
        float[,,] splatmap = new float[mapHeight, mapWidth, layers];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Normaliza para GetSteepness que espera valores 0-1
                float normX = (float)x / (mapWidth  - 1);
                float normY = (float)y / (mapHeight - 1);

                float angle = terrainData.GetSteepness(normX, normY);

                // Blend suavizado en lugar de corte duro
                // Smoothstep devuelve 0 a 1 dentro del rango de transición
                float rockBlend = Mathf.SmoothStep(
                    rockAngleThreshold - blendRange,
                    rockAngleThreshold + blendRange,
                    angle
                );

                // Capa 0: Césped → menos peso donde hay roca
                // Capa 1: Roca   → más peso en pendientes altas
                splatmap[y, x, 0] = 1f - rockBlend;
                splatmap[y, x, 1] = rockBlend;
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmap);
    }
}