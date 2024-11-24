using UnityEngine;

namespace Gameplay.Data.PowerUps
{
    public class PoweUp_GlassOfWater : PowerUp
    {
        public static AnimationCurve s_DefaultFillCurve = null;
        public Color            m_ArrowColor;
        public float 			m_Radius = 6.0f;
        public float			m_FillDuration = 0.3f;
        public AnimationCurve	m_FillCurve;
        private TerrainManager	m_TerrainManager;
        private float           m_RadiusMultiplier;
        private PlayerArrows    m_PlayerArrows;
        private int             m_ArrowIndex;

        protected override void Awake ()
        {
            base.Awake ();

            m_RadiusMultiplier = 1f;
            m_TerrainManager = TerrainManager.Instance;
            m_PlayerArrows = PlayerArrows.Instance;
            m_ArrowIndex = m_PlayerArrows.Register(transform, m_ArrowColor, PlayerArrows.EArrowType.GLASS);

            s_DefaultFillCurve = m_FillCurve;
        }

        public override void OnPlayerTouched (Player _Player)
        {
            m_RadiusMultiplier = Mathf.Clamp(_Player.GetSize() / _Player.GetMinSize(), 1f, 2.5f);
            UnregisterMap();
            m_Model.gameObject.SetActive(false);
            m_ParticleSystem.Play(true);
            m_IdleParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            m_Shadow.SetActive(false);
            m_PlayerArrows.Unregister(m_ArrowIndex);

            m_TerrainManager.FillEmptyCircle(m_Transform.position, m_Radius * m_RadiusMultiplier, m_FillDuration, SelfDestroy);
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}