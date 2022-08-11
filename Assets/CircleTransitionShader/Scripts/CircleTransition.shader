Shader "QuanDZ/CircleTransition"
{
    Properties
    { 
        _Color ("MyColor" , Color ) = (1,1,1,1)
        _Radius("Circle Radius" , Range(0.0 , 1.0)) = 0 
        [ShowAsVector2] _Center("Center Player" , Vector) = (0,0,0,0) 
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexInput //vertext, color , normal , tangent, uv
            {
                float4 vertex: POSITION;
                float2 uv0: TEXCOORD0;
            };

            struct VertextOutput
            {
                float4 vertex: SV_POSITION;
                float2 uv0: TEXCOORD0;
            };

            float4 _Color;
            float _Radius;
            float2 _Center;

            void DrawCircle(in float2 uv, in float2 center, in float radius, in float smoothValue, out float output)
            {
                //in is input, out is value output
                float sqrtDistance = pow( distance(uv , center) , 2);
                float sqrtRadius = pow(radius, 2);

                if(sqrtDistance < radius)
                {
                    /// About smoothstep function : https://developer.download.nvidia.com/cg/smoothstep.html
                    /// Interpolates smoothly from 0 to 1 based on x compared to a and b. (Aka Invert lerp)
                    output = smoothstep(sqrtRadius , sqrtRadius - smoothValue, sqrtDistance);
                }
                else
                {
                    //fill the color
                    output = 0;
                }
            }

            VertextOutput vert (VertexInput v)
            {
                VertextOutput o;
                o.uv0 = v.uv0;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            } 

            fixed4 frag (VertextOutput i) : SV_Target 
            {
                //draw a circle
                float2 center = _Center;
                float smoothValue = 0.01;
                float outputAlpha = 0;
                DrawCircle(i.uv0, center, _Radius, smoothValue, outputAlpha);
                
                return float4(_Color.rgb , 1 - outputAlpha);
            }
            ENDCG
        }
    }
}
