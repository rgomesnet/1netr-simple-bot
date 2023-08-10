﻿using SimpleBotCore.Bot;
using SimpleBotCore.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleBotCore.Logic
{
    public class SimpleBot : BotDialog
    {
        IUserProfileRepository _userProfile;

        public SimpleBot(IUserProfileRepository userProfile)
        {
            _userProfile = userProfile;
        }

        protected async override Task BotConversation()
        {
            SimpleUser user = _userProfile.TryLoadUser(UserId);

            // Create a user if it is null
            if (user == null)
            {
                user = _userProfile.Create(new SimpleUser(UserId));
            }

            await WriteAsync("Boa noite!");

            if (user.Nome != null && user.Idade != 0 && user.Cor != null)
            {
                await WriteAsync(
                    $"{user.Nome}, de {user.Idade} anos, " +
                    $"vejo que cadastrou sua cor preferida como {user.Cor}");
            }


            if (user.Nome == null)
            {
                await WriteAsync("Qual o seu nome?");

                user.Nome = await ReadAsync();

                _userProfile.AtualizaNome(UserId, user.Nome);
            }

            if (user.Idade == 0)
            {
                await WriteAsync("Qual a sua idade?");

                user.Idade = Int32.Parse(await ReadAsync());

                _userProfile.AtualizaIdade(UserId, user.Idade);
            }

            if (user.Cor == null)
            {
                await WriteAsync("Qual a sua cor preferida?");

                user.Cor = await ReadAsync();

                _userProfile.AtualizaCor(UserId, user.Cor);
            }

            await WriteAsync($"{user.Nome}, bem vindo ao Oraculo. Você tem direito a 3 perguntas");

            for (int i = 0; i < 3; i++)
            {
                string texto = await ReadAsync();

                if (texto.EndsWith("?"))
                {
                    await WriteAsync("Processando...");

                    // FAZER: GRAVAR AS PERGUNTAS EM UM BANCO DE DADOS
                    await Task.Delay(5000);

                    _userProfile.AtualizaCor(user.Id, texto);

                    //await WriteAsync("Resposta não encontrada");
                }
                else
                {
                    await WriteAsync("Você disse: " + texto);
                }
            }
        }
    }
}
