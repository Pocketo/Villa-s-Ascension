// FilmEffect.cs - Built-in Pipeline, no necesita nada instalado
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FilmEffect : MonoBehaviour
{
    [Header("Sepia / Grises")]
    [SerializeField][Range(0f, 1f)] private float sepiaIntensity = 1f;
    [SerializeField][Range(0f, 1f)] private float grayIntensity = 1f;

    [Header("Viñeta")]
    [SerializeField][Range(0f, 1f)] private float vignetteIntensity = 0.5f;
    [SerializeField][Range(0f, 1f)] private float vignetteSmoothness = 0.4f;

    [Header("Grain")]
    [SerializeField][Range(0f, 1f)] private float grainIntensity = 0.4f;

    [Header("Parpadeo")]
    [SerializeField] private float flickerSpeed = 8f;
    [SerializeField] private float flickerIntensity = 0.08f;

    private Material mat;

    private const string shaderCode = @"
        Shader ""Hidden/FilmEffect""
        {
            Properties { _MainTex(""Texture"", 2D) = ""white"" {} }
            SubShader
            {
                Pass
                {
                    CGPROGRAM
                    #pragma vertex vert_img
                    #pragma fragment frag
                    #include ""UnityCG.cginc""

                    sampler2D _MainTex;
                    float _Sepia;
                    float _Gray;
                    float _VignetteIntensity;
                    float _VignetteSmoothness;
                    float _GrainIntensity;
                    float _Brightness;
                    float _Time2;

                    float rand(float2 co)
                    {
                        return frac(sin(dot(co, float2(12.9898, 78.233))) * 43758.5453);
                    }

                    fixed4 frag(v2f_img i) : SV_Target
                    {
                        fixed4 col = tex2D(_MainTex, i.uv);

                        // Escala de grises
                        float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                        col.rgb = lerp(col.rgb, float3(gray, gray, gray), _Gray);

                        // Sepia encima del gris
                        float3 sepia;
                        sepia.r = dot(col.rgb, float3(0.393, 0.769, 0.189));
                        sepia.g = dot(col.rgb, float3(0.349, 0.686, 0.168));
                        sepia.b = dot(col.rgb, float3(0.272, 0.534, 0.131));
                        col.rgb = lerp(col.rgb, sepia, _Sepia);

                        // Viñeta
                        float2 center = i.uv - 0.5;
                        float vignette = 1.0 - dot(center, center) * _VignetteIntensity * 4.0;
                        vignette = smoothstep(0.0, _VignetteSmoothness + 0.01, vignette);
                        col.rgb *= vignette;

                        // Grain
                        float grain = rand(i.uv + frac(_Time2)) * _GrainIntensity;
                        col.rgb += grain - (_GrainIntensity * 0.5);

                        // Parpadeo de brillo
                        col.rgb *= _Brightness;

                        return col;
                    }
                    ENDCG
                }
            }
        }";

    void OnEnable()
    {
        mat = new Material(Shader.Find("Hidden/FilmEffect"));
    }

    void OnDisable()
    {
        if (mat != null) DestroyImmediate(mat);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (mat == null)
        {
            Graphics.Blit(src, dst);
            return;
        }

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        float brightness = 1f + (noise - 0.5f) * flickerIntensity;

        mat.SetFloat("_Sepia", sepiaIntensity);
        mat.SetFloat("_Gray", grayIntensity);
        mat.SetFloat("_VignetteIntensity", vignetteIntensity);
        mat.SetFloat("_VignetteSmoothness", vignetteSmoothness);
        mat.SetFloat("_GrainIntensity", grainIntensity);
        mat.SetFloat("_Brightness", brightness);
        mat.SetFloat("_Time2", Time.time);

        Graphics.Blit(src, dst, mat);
    }
}