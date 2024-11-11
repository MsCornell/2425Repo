using System;

namespace Logic.Tests;

public class CharacterTests
{

    private CharacterRepository repository = new("http://localhost:5000/api/Character");

    [Fact]
    public async Task Create_Valid_NoError()
    {
        // arrange
        var character = new Character
        {
            CharacterName = "Jerry"
        };

        // act
        var created = await repository.CreateCharacterAsync(character);
        await repository.DeleteCharacterAsync(created.CharacterName);
        // assert
        Assert.NotNull(created);
        Assert.Equal("Jerry", created.CharacterName);
    }

    [Fact]

    public async Task test(){
        var updated = await repository.UpdateCharacterAsync(new Character{CharacterName = "JasonDamn"}, "Doggy");
        Assert.NotNull(updated);
        Assert.Equal("Doggy", updated.CharacterName);
    }
    
    [Fact]
    //update 
    public async Task Update_Valid_NoError()
    {
        // arrange
        var character = new Character
        {
            CharacterName = "JasonDamn"
        };
        var newCharacter = new Character
        {
            CharacterName = "Doggy"
        };


        // act
        await repository.DeleteCharacterAsync("JasonDamn");
        var created = await repository.CreateCharacterAsync(character);
        var updated = await repository.UpdateCharacterAsync(created, newCharacter.CharacterName);
        await repository.DeleteCharacterAsync(updated.CharacterName);

        // assert
        Assert.NotNull(updated);
        Assert.Equal("Doggy", updated.CharacterName);
    }


}
