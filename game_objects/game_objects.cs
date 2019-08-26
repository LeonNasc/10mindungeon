using System;
using TenMinEvents;

namespace TenMinDungeon
{
    public class Jogador
    {
        protected int _PVs { get; set; }
        protected string[] _ultimas_acoes { get; set; }
        protected int _turno { get; set; }
        protected string _nome { get; set; }
    }
    /// <summary>
    /// Representa um mestre no jogo de RPG
    /// </summary>
    public class Mestre:Jogador
    {
        private BolsaDeEventos _reveses { get; } = new BolsaDeEventos("Reves");
        private BolsaDeEventos _desafios { get; } = new BolsaDeEventos("Desafio");
        private BolsaDeEventos _masmorras { get; } = new BolsaDeEventos("Masmorra");

        /// <summary>
        /// Inicia um novo objeto do tipo <c>Mestre</c>
        /// </summary>
        /// <param name="nome">Nome do mestre inicalizado</param>
        public Mestre(string nome)
        {
            this._turno = 0;
            this._nome = nome;
            this._ultimas_acoes = new string[10];

            Console.WriteLine("---------====---------");
            Console.WriteLine("Seja bem vindo, {0}! Espero que você toque o terror!", nome);
            Console.WriteLine("---------====---------\n");
        }

        /// <summary>
        /// Define a ação do mestre para o turno. 
        /// </summary>
        /// <returns>Um cenário para os heróis</returns>
        public Cenario[] RealizarAcao()
        {
            Console.WriteLine("Qual será a ação do mestre?");

            //Ordem -> Selecionar Desafio, Revés (opcional), Masmorra (Opcional) -> Exibir cenários
            Desafio desafio = (Desafio) this._desafios.ObterNovoEvento();
            Masmorra masmorra = (Masmorra) this._masmorras.ObterNovoEvento();
            Reves reves = (Reves) this._reveses.ObterNovoEvento();

            this.ApresentarDesafio(desafio, masmorra, reves);
            (int base_fis, int base_mag)= this.CalcularDesafio(desafio, masmorra, reves);

            return desafio.ObterCenarios();
        }

        private void ApresentarDesafio(Desafio d, Masmorra m, Reves r)
        {
            Console.WriteLine($"{_nome} preparou o seguinte evento\n");
            Console.WriteLine($"{d._nome} \n -=-=-=-=-=-");
            Console.WriteLine($"{d._descricao} \n -=-=-=-=-=-");
            Console.WriteLine($"Masmorra: {m._nome} : {m._descricao}");
            Console.WriteLine($"Reves: {d._nome} : {d._descricao}");
            Console.WriteLine("-=-=-=-=-=-");
        }
        /// <summary>
        /// Calcula o desafio base do cenário.
        /// </summary>
        /// <returns>Uma tupla com o desafio base fisico e mágico</returns>
        /// <param name="d">O desafio (obrigatório)</param>
        /// <param name="m">A masmorra (obrigatório).</param>
        /// <param name="r">O revés (opcional)</param>
        private (int, int) CalcularDesafio(Desafio d, Masmorra m, Reves r)
        {
            int mod_FIS, mod_MAG;

            (int r_fis, int r_mag) = r.UtilizarEvento();
            (int d_fis, int d_mag) = d.UtilizarEvento();
            (int m_fis,int m_mag) = m.UtilizarEvento();

            mod_FIS = r_fis + d_fis + m_fis;
            mod_MAG = r_mag + d_mag + m_mag;

            return (mod_FIS,mod_MAG);
        }
    }

    public class Heroi:Jogador
    {

        private string _classe { get; set; }
        private string _raca { get; set; }
        private BolsaDeEventos _benesses { get; } = new BolsaDeEventos("Benesses");
        private BolsaDeEventos _items { get; } = new BolsaDeEventos("Espólios");

        public Heroi(string nome, string classe, string raca)
        {
            this._turno = 0;
            this._nome = nome;
            this._classe = classe;
            this._raca = raca;

            Console.WriteLine("Incrível! {0}, o(a) {1} está pronto para aventura!", nome, classe);
        }

        public int EscolherAcao()
        {

            Console.WriteLine("{0}, qual será sua ação para o {1}?", this._nome,this._turno);
            //TODO: Ordem -> Selecionar Benesses/Itens -> Selecionar ação
            return Convert.ToInt32(Console.ReadLine());
        }
    }

    public class Sessao
    {
        private Mestre _mestre { get; set; }
        private Jogador[] _equipe { get; set; }

        public Sessao()
        {

            //Prepara todos os jogadores para a partida
            this.ConfigurarMestre();
            this.ConfigurarEquipe();

       }

        private void ConfigurarMestre()
        {
            //Seta um mestre para a sessão!
            Console.WriteLine("Quem será o mestre?  Insira seu nome");
            this._mestre = new Mestre(Console.ReadLine());
        }

        private void ConfigurarEquipe()
        {
            this._equipe = new Heroi[5];
            string option = "Y";
            int jogadores = 0;

            while (option == "Y" && jogadores < 5)
            {
                Console.WriteLine("Quem será o novo jogador?");
                string nome = Console.ReadLine();
                Console.WriteLine("Qual será a sua classe?");
                string classe = Console.ReadLine();
                Console.WriteLine("Qual será a sua raça?");
                string raca = Console.ReadLine();
                this._equipe[jogadores++] = new Heroi(nome, classe, raca);

                Console.WriteLine("Deseja incluir mais um jogador?");
                option = Console.ReadLine();
            }
            Console.WriteLine("Ok! Todos prontos");
        }

        public void JogarTurno()
        {
            int[] acoes = new int[this._equipe.Length];
            int jog_atual = 0;

            Console.WriteLine("-=-=-=-=-=-=-=-=");
            //Ação do mestre
            Cenario[] cenarios = this._mestre.RealizarAcao();

            for(int i = 0; i<cenarios.Length;i++)
            {
                Console.WriteLine($"{i} - {cenarios[i]._opcao}");
            }

            Console.WriteLine("-=-=-=-=-=-=-=-=");

            //Ação dos jogadores
            foreach (Heroi jogador in this._equipe)
            {
                acoes[jog_atual] = jogador.EscolherAcao();
                Console.WriteLine("-=-=-=-=-=-=-=-=");
                jog_atual++;
            }

            //TODO: ROLAR DADOS
        }

        public void Iniciar()
        {
            //Jogo em si
            this.JogarTurno();
        }
    }
}
