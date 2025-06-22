using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/GameSettings instance")]
public class GameSettings : BaseGameSettings
{
    private static GameSettings m_instance = null;

    public static GameSettings instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = Resources.Load<GameSettings>("Settings/GameSettings");
            }

            return m_instance;
        }
    }

    [Serializable]
    public class Graphic
    {
        [Serializable]
        public class AutoQuality
        {
            public int qualityLevel = -1;
            public int minApiLevel = -1;
            public int minSystemMemory = -1;
            public int minGraphicMemory = -1;
            public int raceFarClipPlane = 2000;
        }

        public List<AutoQuality> autoQualities = new List<AutoQuality>();

        public AutoQuality getAutoQuality(int qualityLevel)
        {
            return autoQualities[Mathf.Max(0, qualityLevel)];
        }
    }

    [Serializable]
    public class RadialGainItemEvent
    {
        public float speed = 10.0f;
        public int eventCount = 10;
        public Vector2 runDelay = new Vector2(0.1f, 0.3f);
        public Vector2 radialLength = new Vector2(0.1f, 0.3f);
        public Vector3 radialAxis = Vector3.back;
        public float radialSmoothTime = 0.1f;
        public float radialMoveEndWait = 0.5f;
    }

    [Serializable]
    public class GainItemEvent
    {
        public float speed = 10.0f;
        public int randAngle = 1;
        public int baseCurrencyCount = 1;
        public float centerPositionRate = 0.0f;
    }

    [Serializable]
    public class Common
    {
        [Serializable]
        public class GradeColor
        {
            public Color Normal = new Color();
            public Color Rare = new Color();
            public Color Epic = new Color();
            public Color Legend = new Color();
            public Color Ultimate = new Color();
            public Color Mythic = new Color();

            public Color getColor(eGrade grade)
            {
                switch (grade)
                {
                    case eGrade.Normal: return Normal;
                    case eGrade.Rare: return Rare;
                    case eGrade.Epic: return Epic;
                    case eGrade.Legendary: return Legend;
                    case eGrade.Ultimate: return Ultimate;
                    case eGrade.Mythic: return Mythic;
                    default:
                        if (Logx.isActive)
                            Logx.trace("Failed getGradeColor {0}", grade);
                        return Color.white;
                }
            }
        }

        public GradeColor gradeColor = new GradeColor();
        public GradeColor upgradeGradeColor = new GradeColor();
    }

    [Serializable]
    public class MainStage
    {
        [Serializable]
        public class CompleteStageEvent
        {
            public float uiFadeInTime = 0.3f;
            public float uiFadeOutTime = 0.3f;
        }

        [Serializable]
        public class AdjustFovToFitScreenResolution
        {
            [SerializeField] float baseScreenWidth = 720.0f;
            [SerializeField] float baseScreenHeight = 1280.0f;
            public float baseFov = 28.0f;
            public float baseSize = 6.0f;

            public float getBaseRatio()
            {
                return baseScreenHeight / baseScreenWidth;
            }
        }

        public AdjustFovToFitScreenResolution adjustFovToFitScreenResolution = new AdjustFovToFitScreenResolution();
        [Tooltip("UI와 오브젝트와의 높이 차이가 이 값 이상일 경우에 오브젝트의 위치를 동기화 한다.")]
        public float uiSyncBoundaryHeight = 0.1f;
        public float planetRotationSpeed = 2.0f;
        public int maxPollutionParticleStartSize = 300;
        public float menuObjectMoveTime = 0.5f;
        public CompleteStageEvent stageCompleteEvent = new CompleteStageEvent();
    }

    [Serializable]
    public class Collection
    {
        [SerializeField] Color enableLevelUpGaugeBarColor = Color.green;
        [SerializeField] Color disableLevelUpGaugeBarColor = Color.white;

        public Color getCountGaugeBarColor(bool isEnable)
        {
            return isEnable ? enableLevelUpGaugeBarColor : disableLevelUpGaugeBarColor;
        }
    }

    [Serializable]
    public class Working
    {
        [Serializable]
        public class RestaurantWork
        {
            public float screenFadeInTime = 2.0f;
            public float InvestorGemCount = 9.0f;
            public float delayNpcLoadTime_straight = 3.0f;
            public float delayNpcLoadTime_across = 15.0f;
            public float delayNpcLoadTime_etc = 30.0f;
            public int straghtPassersbyMaxCount = 3;
            public int acrossPassersbyMaxCount = 1;
            public Color perfectFoodTextColor = new Color(217.0f / 255.0f, 153.0f / 255.0f, 28.0f / 255.0f);
        }

        [Serializable]
        public class EventWork
        {
            public Color GoldlineColor = new Color(1.0f, 191.0f / 255.0f, 0.0f);
            public Color GoldStarColor = new Color(1.0f, 191.0f / 255.0f, 0.0f);
            public Color GemlineColor = Color.magenta;
            public Color GemStarColor = Color.magenta;
        }

        [Serializable]
        public class Result
        {
            [Serializable]
            public class ResultText
            {
                public float waitTime = 0.3f;
            }


            [Serializable]
            public class RewardItems
            {
                public float waitTime = 0.5f;
            }

            [Serializable]
            public class RewardPanel
            {
                public float waitTime = 1.0f;
            }

            public ResultText resultText = new ResultText();
            public RewardPanel rewardPanel = new RewardPanel();
            public float inactiveNormalPlanetTextWaitTime = 1.0f;
            public float defeatTipWaitTime = 1.0f;
            public float buttonShowTime = 1.0f;
        }

        [Serializable]
        public class CameraShake
        {
            [Serializable]
            public class Data
            {
                public float intensity;
                public float time;
                public AnimationCurve shakeCurve = new AnimationCurve();

                public void set(Data data)
                {
                    intensity = data.intensity;
                    time = data.time;
                    shakeCurve = data.shakeCurve;
                }

                public void clear()
                {
                    intensity = 0.0f;
                    time = 0.0f;
                }
            }

            public Data criticalDamage = new Data();
            public Data deadMercenary = new Data();
            // TODO : 2024-02-28 update by pms
            public Data alienRush = new Data();
            //
            public List<Data> nuclearExplosions = new List<Data>();
        }

        public RestaurantWork restaurant = new RestaurantWork();
        public EventWork invest = new EventWork();
        public Result result = new Result();
        public float bulletDefaultRacycastMaxDistance = 20.0f;
        public CameraShake cameraShake = new CameraShake();
        public float deadMotionTime = 1.5f;
        public float dragAndDropInfoShowTime = 3.0f;// TODO : 2024-07-230 by pms
        public float maxSpeedMultiplier = 550.0f;
        public float itemMultiplierWeight = 1.0f;
        public float maxSpeed = 11.0f;
    }

    [Serializable]
    public class Skill
    {
        public float updateTime = 0.1f;
        public float triggerUpdateTime = 0.1f;
    }

    [Serializable]
    public class UI
    {
        [Serializable]
        public class CollectionInfo
        {
            public float selfCloseTime = 5.0f;
            public float modelRotationSpeed = 20.0f;
        }

        [Serializable]
        public class Currency
        {
            public Color notEnoughColor = new Color(1.0f, 0.4313726f, 0.4039216f, 1.0f);
            public Color gemTextColor = new Color(139.0f / 255.0f, 79.0f / 255.0f, 197.0f / 255.0f, 1.0f);
            public Color goldTextColor = new Color(179.0f / 255.0f, 117.0f / 255.0f, 27.0f / 255.0f, 1.0f);
        }

        [Serializable]
        public class ObjectUpgrade
        {
            public Color abilityNameAppliedColorByLab = Color.blue;
        }

        public CollectionInfo collectionInfo = new CollectionInfo();
        public Currency currency = new Currency();
        public ObjectUpgrade objectUpgrade = new ObjectUpgrade();
    }

    [Serializable]
    public class Booster
    {
        public int maxMainBoosterRemainTime = 43200;
    }

    [Serializable]
    public class CameraTransform
    {
        public int stageId;
        public Vector3 position;
        public Quaternion rotation;
        public float fov;
        public float size;
        public float cameraScrollMaxZValue;
        public float cameraScrollMinZValue;
    }

    [SerializeField] int m_oneAdMaxScheduleTime = 5;
    [SerializeField] Graphic m_graphic = new Graphic();
    [SerializeField] RadialGainItemEvent m_radialGainItemEvent = new RadialGainItemEvent();
    [SerializeField] GainItemEvent m_gainItemEvent = new GainItemEvent();
    [SerializeField] Common m_common = new Common();
    [SerializeField] Collection m_collection = new Collection();
    [SerializeField] MainStage m_stage = new MainStage();
    [SerializeField] Skill m_skill = new Skill();
    [SerializeField] UI m_ui = new UI();
    [SerializeField] Booster m_booster = new Booster();
    [SerializeField] float m_buttonEventIntervalWeight = 0.98f;
    [SerializeField] float m_minButtonEventInterval = 0.98f;
    [SerializeField] List<CameraTransform> m_cameraTransforms = new List<CameraTransform>();
    [SerializeField] int m_cameraSettingStageId = 101;
    [SerializeField] float m_cameraScrollMaxTopPositionZValue = 5.0f;
    [SerializeField] float m_cameraScrollMinBottomPositionZValue = 0.0f;
    [SerializeField] Working m_work = new Working();
    [SerializeField] List<int> m_beachTypeMapIds = new List<int>();
    [SerializeField] List<int> m_snowTypeMapIds = new List<int>();
    [SerializeField] List<int> m_shopGoldFoodMulpliers = new List<int>();
    [SerializeField] int m_shopGoldDefaultValue = 135;

    public int oneAdMaxScheduleTime => m_oneAdMaxScheduleTime;
    public Graphic graphic => m_graphic;
    public RadialGainItemEvent radialGainItemEvent => m_radialGainItemEvent;
    public GainItemEvent gainItemEvent => m_gainItemEvent;
    public Common common => m_common;
    public MainStage stage => m_stage;
    public Collection collection => m_collection;

    public Skill skill => m_skill;
    public UI ui => m_ui;
    public Booster booster => m_booster;
    public float buttonEventIntervalWeight => m_buttonEventIntervalWeight;
    public float minButtonEventInterval => m_minButtonEventInterval;
    public List<CameraTransform> cameraTransformSettings => m_cameraTransforms;
    public int cameraSettingStageId => m_cameraSettingStageId;
    public float cameraScrollMaxTopPositionZValue => m_cameraScrollMaxTopPositionZValue;
    public float cameraScrollMinBottomPositionZValue => m_cameraScrollMinBottomPositionZValue;
    public Working work => m_work;

    public List<int> beachTypeMapIds => m_beachTypeMapIds;
    public List<int> snowTypeMapIds => m_snowTypeMapIds;
    public List<int> shopGoldFoodMulpliers => m_shopGoldFoodMulpliers;
    public int shopGoldDefaultValue => m_shopGoldDefaultValue;

#if UNITY_EDITOR
    [MenuItem("Settings/GameSettings")]
    static void Settings()
    {
        Selection.activeObject = instance;
    }

    public static void create()
    {
        if (null == instance)
        {
            var asset = ScriptableObject.CreateInstance<GameSettings>();

            var name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Game/Resources/Settings/GameSettings.asset");
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
