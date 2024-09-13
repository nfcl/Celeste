namespace n_Player.PlayerEventInterface
{

    /// <summary>
    /// 抓住事件接口
    /// </summary>
    public interface ICatchEvent
    {

        /// <summary>
        /// <para/>抓取事件
        /// <para/>当玩家开始抓住继承此接口的物体时会调用此方法
        /// </summary>
        public void OnCatchStart(Player player);
        /// <summary>
        /// <para/>抓取事件
        /// <para/>当玩家抓住继承此接口的物体时会调用此方法
        /// </summary>
        public void OnCatchContinue(Player player);
        /// <summary>
        /// <para/>抓取事件
        /// <para/>当玩家结束抓住继承此接口的物体时会调用此方法
        /// </summary>
        public void OnCatchEnd(Player player);

    };

    public interface IStepOnEvent
    {

        /// <summary>
        /// 当玩家进入物体上方时调用此方法
        /// </summary>
        public void OnStepOn(Player player);
        /// <summary>
        /// 当玩家处于物体上方时调用此方法
        /// </summary>
        /// <param name="player"></param>
        public void OnStepStay(Player player);
        /// <summary>
        /// 当玩家离开物体上方时调用此方法
        /// </summary>
        /// <param name="player"></param>
        public void OnStepLeave(Player player);

    }

    public interface IDash
    {

        public void OnDashStart();

    }

    public interface IDead
    {

        /// <summary>
        /// <para/>玩家死亡事件
        /// <para/>当玩家死亡时调用此接口
        /// </summary>
        public void OnPlayerDead(Player player);

    }

}
