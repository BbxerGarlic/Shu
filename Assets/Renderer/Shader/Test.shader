Shader "Unlit/LineRendererWithCaps"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineWidth("Line Width", Float) = 1.0
        _TextureLength("Texture Length", Float) = 1.0
        _TextureAmount("Texture Amount", Float) = 1.0
        _Touch("Touch", Range(0.1,2)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _LineWidth;
            float _TextureLength;

            float  _Touch;
            
            float _TextureAmount;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // float distance()
            // {
            //     float dist = 
            //     return 
            // }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float dist_x = min(i.uv.x,  _TextureAmount*1.44- i.uv.x) * _TextureLength; // 计算到最近端点的距离
                //float dist_x =  (_TextureAmount*1.44- i.uv.x) * _TextureLength; // 计算到最近端点的距离
                float dist_y = min(i.uv.y, 1 - i.uv.y)*_LineWidth; // 计算到最近端点的距离
                
                //float radius = _Touch;
                float radius = 0.5;

                float alpha= 1;
                
                if(dist_x<radius)
                {
                    if((radius-dist_x)*(radius-dist_x)+(radius-dist_y)*(radius-dist_y)*_Touch<radius*radius)
                    {
                        alpha=1;
                    }else
                    {
                        alpha=0;
                    }
                }
                
            // 使用圆的方程来决定透明度
                //float alpha = step(radius * radius, dist_x * dist_x + dist_y * dist_y);
                //alpha = 1.0 - alpha; // 反转 alpha 值，因为我们希望圆内是可见的
                
                fixed4 col = tex2D(_MainTex, i.uv);

                if(alpha<1.0&&col.a>0)
                {
                    col.a = alpha; // 调整透明度形成半圆效果
                }

                if(col.a>0.2)
                {
                    if(col.b<0.37){
                        col = fixed4(0,0,0,1);}
                    else
                    {
                        col=fixed4(0,0,0,0);
                    }
                }
                else
                {
                    col=fixed4(0,0,0,0);
                }
                
                
                return col;
            }
            ENDCG
        }
    }
}
