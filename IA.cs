using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enum das dificuldades
public enum Dificuldade
{
    fácil, médio, difícil
}
public enum Tipo
{
    Andarilho, Explosivo
}

public class IA : MonoBehaviour
{
    public GameObject alvo;
    public GameObject prefab_explosao;
    public GameObject prefab_spawn_explosao;
    public Dificuldade dificuldade;
    public Tipo tipo_ia;
    public bool vendo_alvo;


    public int vida;
    public float velocidade = 2;
    public float campo_de_visao;
    public float crono = 0;

    //Metodo inicial para setar
    void Start()
    {
	//Acessar as tags para atribuir o jogador como alvo
        alvo = GameObject.FindGameObjectWithTag("Player");
        vida = 100;
    }

    //Metodo de atualizações
    void Update()
    {
        //Se for menor que 1, ele deve se destruir
        if (vida < 1)
        {
            alvo.GetComponent<GameController>().Matou_Vands();
            Destroy(this.gameObject);
        }

        //Condição das dificuldades
        switch (dificuldade)
        {
            case Dificuldade.fácil:
                Movimento(2);
                Visao();
                Verificacao();
                break;
            case Dificuldade.médio:
                Movimento(3);
                Visao();
                Verificacao();
                break;
            case Dificuldade.difícil:
                Movimento(4);
                Visao();
                Verificacao();
                break;
        }

    }

    //Metodo responsavel para controlar o IA
    private void Movimento(float vel)
    {
        //Caso ele esteja vendo o jogador, deve seguir até o mesmo
        if (vendo_alvo)
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(alvo.transform.position.x, 1.39f, alvo.transform.position.z), vel * Time.deltaTime);

        //transform.LookAt(alvo.transform);
        //Metodo para girar o IA para a posição do player
        if (vendo_alvo)
            Olhar();
    }

    //Método para configurar para onde o IA deve esta olhando
    protected void Olhar()
    {
        Vector3 dir = alvo.gameObject.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0; lookRot.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Mathf.Clamp01(3.0f * Time.maximumDeltaTime));

    }

    //Configuração da visão do IA
    private void Visao()
    {
        if (Vector3.Distance(this.transform.position, alvo.transform.position) < campo_de_visao)
        { vendo_alvo = true; }
        else { vendo_alvo = false; }
    }


    //Verifica se o player esta dentro do campo de ataque e começa a ATACAR
    private void Verificacao()
    {

        switch (tipo_ia)
        {
            case Tipo.Andarilho:

                if (Vector3.Distance(this.transform.position, alvo.transform.position) < 1.5f)
                {
                    crono += Time.deltaTime;
                    if (0.6f < crono)
                    {
                        crono = 0;
                        alvo.GetComponent<Player>().Dano(Random.Range(2, 9));
                    }
                }
                break;
            case Tipo.Explosivo:
                if (Vector3.Distance(this.transform.position, alvo.transform.position) < 1.5f)
                {
                    crono += Time.deltaTime;
                    if (0.6f < crono)
                    {
                        crono = 0;
                        alvo.GetComponent<Player>().Dano(Random.Range(9, 16));
                        GameObject f = Instantiate(prefab_explosao, prefab_spawn_explosao.transform.position, prefab_spawn_explosao.transform.rotation);
                        Destroy(this.gameObject);

                    }
                }
                break;
        }

    }
    //Chamar sempre que for causar dano
    public void Dano()
    {
        vida -= Random.Range(5, 20);
    }
}
