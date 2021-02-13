Shader "AdvancedSniper/Scope" 
{
        Properties 
        {
                _Color ("Main Color", Color) = (1,1,1,1)
                _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
                _DecalTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        }
        SubShader 
        {
               	Tags { "RenderType"="Opaque" }
                LOD 200
                                  
                CGPROGRAM
                #pragma surface surf NoLighting

                float4 _Color;
                sampler2D _MainTex;
				sampler2D _DecalTex;

                struct Input 
                {
                        float2 uv_MainTex;
                        float2 uv_DecalTex;
                };

                void surf (Input IN, inout SurfaceOutput o) 
                {       
                		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
						half4 decal = tex2D(_DecalTex, IN.uv_DecalTex);
						c.rgb = lerp (c.rgb, decal.rgb, decal.a);
						c *= _Color;
						o.Albedo = c.rgb;
						o.Alpha = c.a;         
                }

                fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
                {
                        fixed4 c;
                        c.rgb = s.Albedo; 
                        c.a = s.Alpha;
                        return c;
                }
                
                ENDCG
        } 
        FallBack "Diffuse"
}