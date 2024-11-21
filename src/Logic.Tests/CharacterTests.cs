// using System;

// namespace Logic.Tests;

// public class CharacterTests
// {
// // This only works if we don't have a character with the name "X" from other tables
//     private CharacterRepository repository = new("http://localhost:5000/api/Character");

//     [Fact]
//     public async Task DeleteAndRecreate_Valid_NoError()
//     {
//         await repository.DeleteCharacterAsync("X");
//         // arrange
//         var character = new Character
//         {
//             CharacterName = "X"
//         };

//         // act
//         var created = await repository.CreateCharacterAsync(character);
//         // assert
//         Assert.NotNull(created);
//         Assert.Equal("X", created.CharacterName);
//     }
// }
