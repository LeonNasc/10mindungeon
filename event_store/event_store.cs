using System;
using System.Linq;

namespace TenMinEvents
{
/// <summary>
/// Representa um armazém de eventos de jogo.
/// Na versão final, este armazém será utilizado a partir de outro arquivo
/// Utiliza o padrão strategy para o tipo de evento utilizado
/// </summary>
    public class BolsaDeEventos
    {
        private List<Evento> _eventos = new List<Evento>();

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
            return this._eventos.OrderBy(t => Guid.NewGuid()).Take(1)[0];
        }

        /// <summary>
        /// Carrega os eventos a partir da base de eventos
        /// </summary>
        private void CarregarEventos(string tipo_eventos)
        {
            //TODO: Implementar ler de um JSON inicialmente
            while (true)
            {
                this._eventos.add(1);
            }
        }
    }

    /// <summary>
    /// Eventos são coisas que podem ser utilizados
    /// pelo mestre ou jogadores a cada turno
    /// </summary>
    public class Evento
    {
        private string _nome { get; } //Nome do evento
        private string _descricao { get; set; } //Fala sobre o evento
        private string _evento_id { get; } //Identificador único
        private int _raridade { get; set; } //C, R, UR
        private string _img_url { get; } //ilustracao

        /// <summary>
        /// Utiliza um evento no inventório do jogador/mestre
        /// </summary>
        public abstract string UtilizarEvento();
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
    public class Benesse: Evento
    {
        private int _mod { get; } //-n ou +n
        /// <summary>
        /// Tipo da benesse. Apresenta o formato {tipo, limitacao}.
        /// </summary>
        private string _tipo { get; } //Fisica, mágica, raça, classe

        public Benesse(string[] dados_benesse)
        {
            //Destrutura os dados do JSON no objeto instanciado
            this._evento_id = dados_benesse[0];
            this._tipo = dados_benesse[1];
            this._raridade = dados_benesse[2];
            this._nome = dados_benesse[3]
            this._descricao = dados_benesse[4];
            this._mod = dados_benesse[5];
            this._img_url = dados_benesse[6];
        }

        public string[] UtilizarEvento()
        {
            //Retorna o modificador a ser aplicado, a restrição e a descricao
            return [this.]
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
    public class Espolio : Evento
    {
        public Espolio(string[] dados_espolio)
        {

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
    public class Reves: Evento
    {

        public Reves (string[] dados_reves)
        {

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
    /// >O desafio para enganar o dragão é 18 CHAR
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
    public class Desafio: Evento
    {

        public Desafio(string[] dados_desafio)
        {

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
    public class Masmorra : Evento
    {
        public Masmorra(string[] dados_masmorra)
        {

        }
    }
}
