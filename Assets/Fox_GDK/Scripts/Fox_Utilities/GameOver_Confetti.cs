using UnityEngine;

public class GameOver_Confetti : BaseGameBehaviour
{
    public ParticleSystem ps;

    public override void Awake()
    {
        base.Awake();

        gameManager.gameOver_Confetti = this;
    }


    public void Play_Particle()
    {
        ps.Play();
    }

}
