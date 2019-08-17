using System;
using TenMinDungeon;

namespace TenMinDungeon
{
    class Game
    {

        //SampleGameObject[] assets;

        static void Main()
        {

            //Carregar assets
            var greeter = new Splash();
            var game = new Game();

            //Introdução + Logo
            greeter.greet();

            //Carrega jogo
            game.start();
        }

        private Game()
        {
            Console.WriteLine("Estou carregando os dados do jogo");

            //Carrega os dados do jogo
            SampleGameObject objeto = new SampleGameObject();

        }

        ~Game()
        {
            //Limpa os assets e fecha o jogo
            Console.WriteLine("Thanks for playing");
        }

        private void start()
        {

            Sessao sessao = new Sessao();
        }
    }

    class Splash
    {

        public Splash()
        {
            Console.WriteLine("Imagina uma logo bem bonita");
        }

        public void greet()
        {

            Console.WriteLine("Agora está dando oi essa logo");
        }
    }
}
