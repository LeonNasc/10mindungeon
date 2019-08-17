using System;

namespace TenMinDungeon
{
    /// <summary>
    /// Representa um mestre no jogo de RPG
    /// </summary>
    public class Mestre
    {

        public int PVs { get; set; }
        public string[] ultimas_acoes;
        private int turno;
        public string nome;

        /// <summary>
        /// Inicia um novo objeto do tipo <c>Mestre</c>
        /// </summary>
        /// <param name="nome">Nome do mestre inicalizado</param>
        public Mestre(string nome)
        {
            this.turno = 0;
            this.nome = nome;
            this.ultimas_acoes = new string[20];

            Console.WriteLine("Seja bem vindo, {0}! Espero que você toque o terror!", nome);
        }

        public string realizarAcao()
        {
            Console.WriteLine("Qual será a ação do mestre?");
            //TODO: Ordem -> Selecionar Desafio, Revés (opcional), Masmorra (Opcional) -> Exibir cenários
            return ultimas_acoes[this.turno];
        }

    }

    public class Jogador
    {

        public int PVs { get; set; }
        public string nome;
        private int turno;
        public string classe { get; set; }

        public Jogador(string nome, string classe)
        {
            this.turno = 0;
            this.nome = nome;
            this.classe = classe;

            Console.WriteLine("Incrível! {0}, o(a) {1} está pronto para aventura!", nome, classe);
        }

        public int escolherAcao()
        {

            Console.WriteLine("{0}, qual será sua ação para o {1}?", this.nome,this.turno);
            //TODO: Ordem -> Selecionar Benesses/Itens -> Selecionar ação
            return Convert.ToInt32(Console.ReadLine());

        }
    }

    public class Sessao
    {
        public Mestre mestre { get; set; }
        public Jogador[] equipe;

        public Sessao()
        {

            //Prepara todos os jogadores para a partida
            this.configurarMestre();
            this.configurarEquipe();

            //Jogo em si
            this.jogarTurno();
        }

        private void configurarMestre()
        {
            //Seta um mestre para a sessão!
            Console.WriteLine("Quem será o mestre?  Insira seu nome");
            this.mestre = new Mestre(Console.ReadLine());

        }

        private void configurarEquipe()
        {
            this.equipe = new Jogador[5];
            string option = "Y";
            int jogadores = 0;

            while (option == "Y" && jogadores < 5)
            {
                Console.WriteLine("Quem será o novo jogador?");
                string nome = Console.ReadLine();
                Console.WriteLine("Qual será a sua classe?");
                string classe = Console.ReadLine();
                this.equipe[jogadores++] = new Jogador(nome, classe);

                Console.WriteLine("Deseja incluir mais um jogador?");
                option = Console.ReadLine();
            }
            Console.WriteLine("Ok! Todos prontos");
        }

        public void jogarTurno()
        {
            int[] acoes = new int[this.equipe.Length];
            int jog_atual = 0;

            Console.WriteLine("-=-=-=-=-=-=-=-=");
            //Ação do mestre
            this.mestre.realizarAcao();

            Console.WriteLine("-=-=-=-=-=-=-=-=");

            //Ação dos jogadores
            foreach (Jogador jogador in equipe)
            {
                acoes[jog_atual] = jogador.escolherAcao();
                Console.WriteLine("-=-=-=-=-=-=-=-=");
                jog_atual++;
            }

            //TODO: ROLAR DADOS
        }
    }
}
