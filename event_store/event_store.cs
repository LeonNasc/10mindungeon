using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TenMinEvents
{
    public interface IEvento
    {
        void UtilizarEvento(); 
    }
    /// <summary>
    /// Representa um armazém de eventos de jogo.
    /// Na versão final, este armazém será utilizado a partir de outro arquivo
    /// Utiliza o padrão strategy para o tipo de evento utilizado
    /// </summary>
    public class BolsaDeEventos
    {
        private List<Evento> _eventos { get; set; } = new List<Evento>();

        /// <summary>
        /// Construtor padrão: Obtem um tipo de evento e permite
        /// aos jogadores obter eventos deste tipo
        /// </summary>
        /// <param name="tipo_eventos">Tipo do eventos(Revés, Habilidade, Item,...)</param>
        public BolsaDeEventos(string tipo_eventos)
        {
            this.CarregarEventos(tipo_eventos);
        }
        /// <summary>
        /// Obtém um novo evento quando solicitado
        /// </summary>
        /// <returns>The novo evento.</returns>
        public Evento ObterNovoEvento()
        {
            //return Enumerable.Take(_eventos.OrderBy(t => Guid.NewGuid()), 1).ToArray()[0];
            Console.WriteLine(_eventos.ToArray().Length);
            return _eventos.ToArray()[0];
        }

        /// <summary>
        /// Carrega os eventos a partir da base de eventos
        /// <param name="tipo_eventos"> Tipo do evento a ser carregado </param>
        /// </summary>
        private void CarregarEventos(string tipo_eventos)
        {
            string path = $"{Directory.GetCurrentDirectory()}/../../{tipo_eventos}.csv";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string evento;
                    while ((evento = sr.ReadLine()) != null)
                    {
                        if (evento.Split(',')[0] == "id")
                        {
                            continue;
                        }
                        try {
                            switch (tipo_eventos)
                            {
                                case "Benesse":
                                    _eventos.Add(new Benesse(evento.Split(',')));
                                    break;
                                case "Espólio":
                                    _eventos.Add(new Espolio(evento.Split(',')));
                                    break;
                                case "Reves":
                                    _eventos.Add(new Reves(evento.Split(',')));
                                    break;
                                case "Desafio":
                                    _eventos.Add(new Desafio(evento.Split(',')));
                                    break;
                                case "Masmorra":
                                    _eventos.Add(new Masmorra(evento.Split(',')));
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch(Exception objErr)
                        {
                            Console.WriteLine(objErr);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"O arquivo {tipo_eventos}.csv não existe");
            }
        }
    }

    /// <summary>
    /// Eventos são coisas que podem ser utilizados
    /// pelo mestre ou jogadores a cada turno
    /// </summary>
    public class Evento:IEvento
    {
        protected string _evento_id { get; set; }//Identificador único
        public string _nome { get; set; }  //Nome do evento
        protected string _tipo { get; set; } //Fisica, mágica, raça, classe
        public string _descricao { get; set; } //Fala sobre o evento
        protected string _raridade { get; set; }//C, R, UR
        protected string _img_url { get; set; }//ilustracao
        protected string _mod_corpo { get; set; } //-n ou +n
        protected string _mod_mente { get; set;} // -n ou +n

        /// <summary>
        /// Utiliza um evento no inventório do jogador/mestre
        /// </summary>
        public virtual void UtilizarEvento()
        {
            throw new NotImplementedException();
        }
    }

    public class Utilizavel: Evento
    {
        protected string[] _limitacoes;

        /// <summary>
        /// Utiliza a benesse após verificar se o jogador é capaz de usá-la
        /// </summary>
        /// <returns>o nome, o modificador e a descrição </returns>
        public (int,int) UtilizarEvento(string dado_jogador)
        {
            //Retorna o modificador a ser aplicado, a restrição e a descricao
            if (this.ValidarUtilizavel(dado_jogador.Split(','))){
                return (Convert.ToInt32(_mod_corpo),Convert.ToInt32(_mod_mente));
            }
            else {
                //TODO: Lançar exceçao
                Console.WriteLine($"O jogador não pode utilizar: {_nome}");
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Valida se o jogador pode utilizar o evento
        /// </summary>
        /// <returns><c>true</c>, se for possível, <c>false</c> se não.</returns>
        /// <param name="dados_jogador">Dados do jogador.</param>
        private Boolean ValidarUtilizavel(string[] dados_jogador)
        {
            foreach (string dado_jogador in dados_jogador)
            {
                foreach (string limitacao in this._limitacoes)
                {
                    if (dado_jogador == limitacao)
                        return true;
                }
            }
            return false;
        }
    }

    public class Cena : Evento
    {
        public new void UtilizarEvento()
        {
            return;
        }
    }

    /// <summary>
    /// Benesses são eventos atrelados ao jogador.
    /// </summary>
    /// <example>
    /// João é um guerreiro orc.
    /// Em um turno obtém a benesse "Força intimidadora" 
    /// A benesse garante +4 na rolagem de uma situação que 
    /// envolva força ou intimidação 
    /// </example>
    /// <remarks>
    /// Benesses são geradas no momento da escolha da classe e raça 
    /// E a cada 3 níveis (4, 7, 10)
    /// </remarks>
    public class Benesse: Utilizavel
    {
        public Benesse(string[] dados_benesse)
        {
            //Destrutura os dados do JSON no objeto instanciado
            this._evento_id = dados_benesse[0];
            this._nome = dados_benesse[1];
            this._tipo = dados_benesse[2];
            this._raridade = dados_benesse[3];
            this._limitacoes = dados_benesse[4].Split(',');
            this._descricao = dados_benesse[5];
            this._mod_corpo = dados_benesse[6];
            this._mod_mente = dados_benesse[7];
            this._img_url = dados_benesse[8];
        }
    }

    /// <summary>
    /// Espolios são como benesses, mas podem ser trocados entre jogadores
    /// (durante a partida) ou por ouro (no cenário final)
    /// </summary>
    /// <example>
    /// Ao vencer um desafio, João, o guerreiro orc recebeu o espólio
    /// "Varinha de gelo frozen", mas somente classes mágicas podem utilizar.
    /// 
    /// Ele decide trocar o item por um nível com Joana, a feiticeira.
    ///
    /// Assim, João fica mais próximo de seu objetivo, mas concede uma vantagem
    /// para Joana.
    /// </example>
    /// <remarks>
    /// Espolios são concedidos quando um jogador supera um desafio (ganha um nível) 
    /// </remarks>
    public class Espolio : Utilizavel
    {
        private int _preco { get; set; }

        public Espolio(string[] dados_espolio)
        {
            //Destrutura os dados do JSON no objeto instanciado
            this._evento_id = dados_espolio[0];
            this._nome = dados_espolio[1];
            this._tipo = dados_espolio[2];
            this._raridade = dados_espolio[3];
            this._limitacoes = dados_espolio[4].Split(',');
            this._descricao = dados_espolio[5];
            this._mod_corpo = dados_espolio[6];
            this._mod_mente = dados_espolio[7];
            this._img_url = dados_espolio[8];
            this._preco = Convert.ToInt32(dados_espolio[9]);
        }
    }

     /// <summary>
     /// Desafios são colocados a cada turno pelo mestre para gerar 
     /// dificuldades para os jogadores.
     /// </summary>
     /// <example>
     /// Os heróis com capacidade física estão acabando com seus desafios!
     /// O mestre decide colocar um dragão com escamas de pedra.
     /// 
     /// >O desafio para enfrentar o dragão é 20 FIS/16 MAG
     /// >O desafio para enganar o dragão é 22 MAG
     /// >O desafio para despistar o dragão é 18 FIS/18 MAG
     /// >Fugir tem um desafio de 20
     ///
     /// </example>
     /// <remarks>
     /// Desafios são um dos três itens que compôem um cenário 
     /// Podem ser monstros ou dificuldades em geral (alarmes, portas)
     ///
     /// Apresentam opções que devem ser escolhidas pelos jogadores
     /// </remarks>
    public class Desafio : Cena
    {
        private string _mod_base { get; set; }
        private string _texto_base { get; set; }
        private string _texto_corpo { get; set; }
        private string _texto_mente { get; set; }

        public Desafio(string[] dados_desafio)
        {
            this._evento_id = dados_desafio[0];
            this._nome = dados_desafio[1];
            this._raridade = dados_desafio[2];
            this._descricao = dados_desafio[3];
            this._mod_base = dados_desafio[4];
            this._mod_corpo = dados_desafio[5];
            this._mod_mente = dados_desafio[6];
            this._texto_base = dados_desafio[7];
            this._texto_corpo = dados_desafio[8];
            this._texto_mente = dados_desafio[9];
            this._img_url = dados_desafio[10];
        }

        /// <summary>
        /// Obtém as opções de jogo para os jogadores de um desafio
        /// </summary>
        /// <returns>Os cenarios.</returns>
        /// <remarks>
        /// Existem 3 tipos de desafio:
        /// + Fisico (Derrubar uma porta)
        /// + Magico (Utilizar uma magia de destravamento)
        /// + Alternativo (Tentar abrir a fechadura)  
        /// </remarks>
        public Cenario[] ObterCenarios()
        {
            Cenario desafio_corpo = new Cenario(_texto_corpo, Int32.Parse(_mod_corpo));
            Cenario desafio_mente = new Cenario(_texto_mente, Int32.Parse(_mod_mente));
            Cenario desafio_base = new Cenario(_texto_base,Int32.Parse(_mod_base));
            Cenario fuga = new Cenario();

            return new Cenario[] { desafio_corpo, desafio_mente, desafio_base, fuga };
        }

        /// <summary>
        /// Utilizado para calcular o desafio base
        /// </summary>
        /// <returns>O desafio base para desafio.</returns>
        public new(int, int) UtilizarEvento()
        {
            Console.WriteLine($"{ _nome} foi selecionado!");
            return (1, 1);
        }
    }

    /// <summary>
    /// Modelo padrão de cenário para desafios
    /// </summary>
    public class Cenario
    {
        public string _opcao { get; set; } = "A equipe tenta fugir";
        public int _modificador { get; set; } = 10;

        public Cenario() { }

        public Cenario(string opcao, int modificador)
        {
            this._opcao = opcao;
            this._modificador = modificador;
        }
    }

    /// <summary>
    /// Reveses são colocados pelo mestre para dificultar o jogo
    /// </summary>
    /// <example>
    /// Em um turno, os heróis estão progredindo pela dungeon e encontram um
    /// dragão. O mestre deseja que a luta seja mais difícil para magos e escolhe
    /// o revés: "Proteção mágica"
    /// 
    /// O revés aumenta em +3 o nível de dificuldade do desafio para os 
    /// jogadores de classes mágicas
    /// </example>
    /// <remarks>
    /// Reveses são um dos três itens que compõem um cenário 
    /// </remarks>
    public class Reves : Cena
    {
        public Reves(string[] dados_reves)
        {
            this._evento_id = dados_reves[0];
            this._nome = dados_reves[1];
            this._raridade = dados_reves[2];
            this._descricao = dados_reves[3];
            this._mod_corpo = dados_reves[4];
            this._mod_mente = dados_reves[5];
            this._img_url = dados_reves[6];

        }

        public new (int,int) UtilizarEvento(){
            Console.WriteLine($"{ _nome} foi selecionado!");
            return (1,1);
        }
    }


    /// <summary>
    /// Masmorras são os locais onde os desafios ocorrem.
    /// </summary>
    /// <example>
    /// 
    /// A equipe é composta por guerreiros fortes e magos sábios...
    /// Porém todos são pesados ou frágeis.
    /// 
    /// O Mestre escolhe a Masmorra "Floresta vertical" para a batalha.
    /// 
    /// O desafio aumenta em +5 para todos os membros da equipe
    ///  
    /// Se um elfo fosse parte da equipe, 
    /// o desafio para ele seria reduzido em  -5
    /// </example>
    /// <remarks>
    /// Masmorras são um dos três itens que compõem um cenário 
    /// </remarks>
    public class Masmorra : Cena
    {
        public Masmorra(string[] dados_masmorra)
        {
            this._evento_id = dados_masmorra[0];
            this._nome = dados_masmorra[1];
            this._raridade = dados_masmorra[2];
            this._descricao = dados_masmorra[3];
            this._mod_corpo = dados_masmorra[4];
            this._mod_mente = dados_masmorra[5];
            this._img_url = dados_masmorra[6];
        }

        /// <summary>
        /// Afeta o modificador do desafio
        /// </summary>
        /// <returns>O modificador para o desafio base.</returns>
        public new(int, int) UtilizarEvento()
        {
            Console.WriteLine($"{ _nome} foi selecionado!");
            return (1, 1);
        }
    }
}
