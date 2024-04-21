using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UISystem : Singleton<UISystem>
{
    public const int IMAGE_BASE_SCALE = 512;
    public const int IMAGE_BASE_ROUND = 24;

    [SerializeField] private Sprite m_defaultElevationSprite;

    protected override void Awake()
    {
        base.Awake();

        create_defaule_elevationSprite();
    }

    private void create_defaule_elevationSprite()
    {
        int diameter = IMAGE_BASE_SCALE; // 직경 설정
        Texture2D texture = new Texture2D(diameter, diameter);
        float radius = diameter / 2f;
        Vector2 center = new Vector2(radius, radius);

        // 각 픽셀에 대해 그라데이션 적용
        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                // 그라데이션 계산: 중심에서 거리에 따른 투명도 적용
                float gradient = distance / radius;
                gradient = Mathf.Clamp01(gradient);

                // 중앙은 하얀색, 가장자리는 완전 투명
                Color color = new Color(1f, 1f, 1f, 1f - gradient);
                texture.SetPixel(x, y, color);
            }
        }

        // 텍스처 변경사항 적용
        texture.Apply();

        // Texture2D에서 Sprite 생성
        m_defaultElevationSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
     
        // 9-Slice 처리를 위한 Border 설정
        // 여기서는 이미지의 중심을 유지하고 가장자리를 확장할 수 있도록 Border 값을 설정합니다.
        // Border 값은 (left, bottom, right, top) 형태로 설정됩니다.
        Vector4 border = new Vector4(radius - IMAGE_BASE_ROUND, radius - IMAGE_BASE_ROUND, radius - IMAGE_BASE_ROUND, radius - IMAGE_BASE_ROUND);

        // Texture2D에서 9-Slice Sprite 생성
        m_defaultElevationSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect, border);
    }
    
    public Sprite get_elevationSprite()
    {
        return m_defaultElevationSprite;
    }
}
