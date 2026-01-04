using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InGameUIController : MonoSingleton<InGameUIController>
{

    [Header("开始死亡效果")]
    [SerializeField] FadeScene _fadeScene;
    [SerializeField] GameObject _deathText;
    [SerializeField] GameObject _restartGameBt;

    [SerializeField] Slider _playerSlider;

    [Header("游戏货币")]
    [SerializeField] TextMeshProUGUI _coldCoinText;
    [SerializeField] TextMeshProUGUI _soulText;

    [Header("图片冷却")]
    [SerializeField] Image _dashImage;
    [SerializeField] Image _blackHoleImage;
    [SerializeField] Image _crystalImage;
    [SerializeField] Image _parryImage;
    [SerializeField] Image _flaskImage;

    [SerializeField] List<Sprite> _skillSprites=new();

    Player _player;
    SkillManager _skillManager;

    float _dashCooldownTime;
    float _blackHoleCooldownTime;
    float _crystalCooldownTime;
    float _parryCooldownTime;
    float _flaskCooldownTime;

    private void Start()
    {

        _player = GlobalReferencesManager.Instance.GamePlayer;
        _skillManager = SkillManager.Instance;

        InitInGameUI();
    }

    private void Update()
    {
        ImageCooldownTimer(_dashImage, _dashCooldownTime);
        ImageCooldownTimer(_blackHoleImage, _blackHoleCooldownTime);
        ImageCooldownTimer(_crystalImage, _crystalCooldownTime);
        ImageCooldownTimer(_parryImage, _parryCooldownTime);
        ImageCooldownTimer(_flaskImage, _flaskCooldownTime);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventUnsubscribe();
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitInGameUI()
    {
        if (_skillManager != null)
        {
            _dashCooldownTime = _skillManager.dashSkill.Cooldown;
            _blackHoleCooldownTime = _skillManager.BlackSkill.Cooldown;
            _crystalCooldownTime = _skillManager.CrystalSkill.Cooldown;
            _parryCooldownTime = _skillManager.ParrySkill.Cooldown;
        }

        UpdateGoldCoinHandle();
        UpdateSoulHandle();

        EventSubscription();
    }

    /// <summary>
    /// 事件订阅
    /// </summary>
    private void EventSubscription()
    {
        if (_player == null)
            return;

        if (_player.Attribute != null)
        {
            _player.Attribute.OnChangeHealthEvent += UpdatePlayerHealthUIHandle;
        }

        if(_player.CurrencyData != null)
        {
            _player.CurrencyData.OnGoldCoinUpdateEvent += UpdateGoldCoinHandle;
            _player.CurrencyData.OnSoulUpdateEvent += UpdateSoulHandle;
        }
    }

    /// <summary>
    /// 取消事件订阅
    /// </summary>
    private void EventUnsubscribe()
    {
        if (_player == null)
            return;

        if (_player.Attribute != null)
        {
            _player.Attribute.OnChangeHealthEvent -= UpdatePlayerHealthUIHandle;
        }

        if (_player.CurrencyData != null)
        {
            _player.CurrencyData.OnGoldCoinUpdateEvent -= UpdateGoldCoinHandle;
            _player.CurrencyData.OnSoulUpdateEvent -= UpdateSoulHandle;
        }
    }

    #region 更新数据

    /// <summary>
    /// 更新生命值UI
    /// </summary>
    private void UpdatePlayerHealthUIHandle()
    {
        _playerSlider.maxValue = _player.Attribute.GetMaxHealth();
        _playerSlider.value = _player.Attribute.CurrentHealth;
    }

    /// <summary>
    /// 更新金币
    /// </summary>
    private void UpdateGoldCoinHandle()=> _coldCoinText.text = "GoldCoin:" + _player.CurrencyData.GoldCoin;

    /// <summary>
    /// 更新灵魂
    /// </summary>
    private void UpdateSoulHandle()=>_soulText.text="Soul:"+_player.CurrencyData.Soul;

    #endregion

    #region 设置图标数据

    /// <summary>
    /// 设置冲刺图片数据
    /// </summary>
    public void SetDashImageData() => SetImageData(_dashImage, _skillSprites[0]);
    

    /// <summary>
    /// 设置黑洞图片数据
    /// </summary>
    public void SetBlackHoleImageData()=>SetImageData(_blackHoleImage, _skillSprites[1]);
    

    /// <summary>
    /// 设置水晶图片数据
    /// </summary>
    public void SetCrystalImageData()=>SetImageData(_crystalImage, _skillSprites[2]);
    

    /// <summary>
    /// 设置反击图片数据
    /// </summary>
    public void SetParryImageData()=>SetImageData(_parryImage, _skillSprites[3]);

    /// <summary>
    /// 设置药品图片数据
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="cooldown"></param>
    public void SetFlaskImageData(Sprite sprite,float cooldown)
    {
        _flaskImage.sprite = sprite;
        _flaskCooldownTime = cooldown;
    }
    

    /// <summary>
    /// 设置图片数据
    /// </summary>
    /// <param name="image"></param>
    /// <param name="sprite"></param>
    private void SetImageData(Image image,Sprite sprite)
    {
        if (image != null)
        {
            Transform parent = image.transform.parent;
            if(parent != null)
            {
                parent.GetComponent<Image>().sprite = sprite;
            }

            image.sprite = sprite;
        }
    }

    #endregion

    #region 设置图标冷却
    /// <summary>
    /// 设置冲刺技能图片冷却
    /// </summary>
    public void DashImageCooldown()=> SetImageCooldown(_dashImage);

    /// <summary>
    /// 设置黑洞技能图片冷却
    /// </summary>
    public void BlackHoleImageCooldown()=> SetImageCooldown(_blackHoleImage);


    /// <summary>
    /// 设置水晶技能图片冷却
    /// </summary>
    public void CrystalImageCooldown() => SetImageCooldown(_crystalImage);

    /// <summary>
    /// 设置反击技能图片冷却
    /// </summary>
    public void ParryImageCooldown()=> SetImageCooldown(_parryImage);


    /// <summary>
    /// 设置药品图片冷却
    /// </summary>
    public void FlaskImageCooldown()=> SetImageCooldown(_flaskImage);

    /// <summary>
    /// 设置技能图片冷却
    /// </summary>
    /// <param name="image"></param>
    private void SetImageCooldown(Image image)
    {
        if (image != null)
        {
            if (image.fillAmount <= 0)
            {
                image.fillAmount = 1;
            }
        }
    }

    /// <summary>
    /// 设置技能倒计时
    /// </summary>
    /// <param name="image"></param>
    /// <param name="cooldown"></param>
    private void ImageCooldownTimer(Image image, float cooldown)
    {
        if (image == null)
            return;

        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }

    #endregion


    /// <summary>
    /// 玩家死亡特效
    /// </summary>
    public void PlayerDeathEffect()
    {
        MenuController.Instance.SwitchMenu(null);

        _fadeScene.gameObject.SetActive(true);
        _fadeScene.FadeOutScene();

        StartCoroutine(ShowDeathTextCo());
    }

    IEnumerator ShowDeathTextCo()
    {
        yield return new WaitForSeconds(1f);
        _deathText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _restartGameBt.SetActive(true);
    }

    
}
