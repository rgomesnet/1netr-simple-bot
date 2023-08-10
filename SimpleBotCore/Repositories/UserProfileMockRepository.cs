﻿using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBotCore.Logic;
using System;
using System.Collections.Generic;

namespace SimpleBotCore.Repositories
{
    public class UserProfileMockRepository : IUserProfileRepository
    {
        Dictionary<string, SimpleUser> _users = new Dictionary<string, SimpleUser>();
        private readonly IMongoCollection<BsonDocument> _collection;

        public UserProfileMockRepository()
        {
            var client = new MongoClient("mondb://localhost:27107");
            var database = client.GetDatabase("chat_bot");
            _collection = database.GetCollection<BsonDocument>("user_questions");
        }


        public SimpleUser TryLoadUser(string userId)
        {
            if (Exists(userId))
            {
                return GetUser(userId);
            }

            return null;
        }

        public SimpleUser Create(SimpleUser user)
        {
            if (Exists(user.Id))
                throw new InvalidOperationException("Usuário ja existente");

            SaveUser(user);

            return user;
        }

        public void AtualizaNome(string userId, string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (!Exists(userId))
                throw new InvalidOperationException("Usuário não existe");

            var user = GetUser(userId);

            user.Nome = name;

            SaveUser(user);
        }

        public void AtualizaIdade(string userId, int idade)
        {
            if (idade <= 0)
                throw new ArgumentOutOfRangeException(nameof(idade));

            if (!Exists(userId))
                throw new InvalidOperationException("Usuário não existe");

            var user = GetUser(userId);

            user.Idade = idade;

            SaveUser(user);
        }

        public void AtualizaCor(string userId, string cor)
        {
            if (cor == null)
                throw new ArgumentNullException(nameof(cor));

            if (!Exists(userId))
                throw new InvalidOperationException("Usuário não existe");

            var user = GetUser(userId);

            user.Cor = cor;

            SaveUser(user);
        }

        private bool Exists(string userId)
        {
            return _users.ContainsKey(userId);
        }

        private SimpleUser GetUser(string userId)
        {
            return _users[userId];
        }

        private void SaveUser(SimpleUser user)
        {
            _collection.InsertOne(new BsonDocument
            {
                {"id", user.Id },
                {"nome", user.Nome },
                {"cor", user.Cor }
            });
        }
    }
}

