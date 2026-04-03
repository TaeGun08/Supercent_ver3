namespace Project.Core.Interfaces
{
    public interface INpcState
    {
        void Enter();    // 상태 진입 시 1회 실행
        void Execute();  // 매 프레임 실행 (Update 역할)
        void Exit();     // 상태 퇴장 시 1회 실행
    }
}
