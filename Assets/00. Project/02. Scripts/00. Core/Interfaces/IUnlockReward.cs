namespace Project.Core.Interfaces
{
    public interface IUnlockReward
    {
        /// <summary>
        /// 해금이 완료되었을 때 실행될 구체적인 보상 로직
        /// </summary>
        void Execute();
    }
}
