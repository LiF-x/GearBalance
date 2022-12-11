/**
* <author>Christophe Roblin</author>
* <url>lifxmod.com</url>
* <credits>Christope Roblin for building addition process with working thumbnails</credits>
* <description>This will add new building models for GearBalance Life is Feudal: Your Own servers</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*/
// test
// $LiFx::hooks::onSpawnCallbacks =  JettisonArray();
// $LiFx::hooks::onConnectCallbacks =  JettisonArray();
// $LiFx::hooks::onDisconnectCallbacks =  JettisonArray();

// $LiFx::hooks::onDeathCallbacks =  JettisonArray();
// $LiFx::hooks::onKillCallbacks =  JettisonArray();

// $LiFx::hooks::onJHStartCallbacks =  JettisonArray();
// $LiFx::hooks::onJHEndCallbacks =  JettisonArray();
// $LiFx::hooks::onCharacterCreateCallbacks =  JettisonArray();

// $LiFx::hooks::onStartCallbacks =  JettisonArray();
// $LiFx::hooks::onPostInitCallbacks  =  JettisonArray();
// $LiFx::hooks::onInitServerCallbacks  =  JettisonArray();
// $LiFx::hooks::onInitServerDBChangesCallbacks  =  JettisonArray();

// art\models\3d\construction\fortifications\stone_wall4m_gate_middle

if (!isObject(GearBalance))
{
    new ScriptObject(GearBalance)
    {
    };
}

package GearBalance
{
  function GearBalance::setup() {

    LiFx::registerCallback($LiFx::hooks::onSpawnCallbacks, gmCharacter, GearBalance);
    LiFx::registerCallback($LiFx::hooks::onInitServerDBChangesCallbacks, gmCharacterTable, GearBalance);


    LiFx::registerCallback($LiFx::hooks::onInitServerDBChangesCallbacks, effectstable, GearBalance);

    LiFx::registerCallback($LiFx::hooks::onInitServerDBChangesCallbacks, outfitnames, GearBalance);

    LiFx::registerCallback($LiFx::hooks::onInitServerDBChangesCallbacks, ringstack, GearBalance);


    LiFx::registerCallback($LiFx::hooks::onPostInitCallbacks, RegisterBasil, GearBalance);

    //LiFx::registerCallback($LiFx::hooks::onPostInitCallbacks, ExecDatablock, GearBalance);
  }

  function GearBalance::RegisterBasil() {

    BasilMod::pack_content("data/cm_effects.xml");
    BasilMod::pack_content("data/cm_messages.xml"); 
    BasilMod::pack_content("data/cm_equipTypes.xml"); 
    BasilMod::pack_content("data/skill_types.xml"); 
    BasilMod::pack_content("mods/GearBalance/items/icons/Light_Armour.png"); 
    BasilMod::pack_content("mods/GearBalance/items/icons/Light_Armour_u.png"); 
  }

  function GearBalance::effectstable() {
    //changing secondary armor effects to other things
    dbi.Update("UPDATE `effects` SET  PlayerEffectID = 35 WHERE ID = 28");    //impact to leatherworking skills
    dbi.Update("UPDATE `effects` SET  PlayerEffectID = 5 WHERE ID = 30");    //reload to slowed
    //dbi.Update("UPDATE `effects` SET  PlayerEffectID = XX WHERE ID = 32");    would change spurs to XX
    //dbi.Update("UPDATE `effects` SET  PlayerEffectID = XX WHERE ID = 34");    would change increased protection to XX
    //dbi.Update("UPDATE `effects` SET  PlayerEffectID = XX WHERE ID = 36");    would change tightly fitted armor to XX
    dbi.Update("UPDATE `effects` SET  Effect_name = 'Skills Raised: Leatherworking' WHERE ID = 28");    //renames leatherwork effect icon
    dbi.Update("UPDATE `effects` SET  Effect_name = 'Restricted Movement' WHERE ID = 30");    //renames slow effect
    dbi.Update("UPDATE `effects` SET  Effect_name = 'Skills Raised: Metalwork and Geology' WHERE ID = 21");    //renames smith apron effect icon
    dbi.Update("UPDATE `effects` SET  Effect_name = 'Skills Raised: Building' WHERE ID = 24");    //renames siege apron effect icon
    dbi.Update("UPDATE `effects` SET  Effect_name = 'Skills Raised: Farming' WHERE ID = 25");    //renames cook apron effect icon
  }

  function GearBalance::outfitnames() {
    //renaming skill outfit objects to new skill groups
    dbi.Update("UPDATE `objects_types` SET  Name = 'Leatherworking Outfit' WHERE ID = 1580");
    dbi.Update("UPDATE `objects_types` SET  Name = 'Metalworking and Geology Outfit' WHERE ID = 303");
    dbi.Update("UPDATE `objects_types` SET  Name = 'Carpentry Outfit' WHERE ID = 304");
    dbi.Update("UPDATE `objects_types` SET  Name = 'Alchemy Outfit' WHERE ID = 305");
    dbi.Update("UPDATE `objects_types` SET  Name = 'Building Outfit' WHERE ID = 306");
    dbi.Update("UPDATE `objects_types` SET  Name = 'Farming Outfit' WHERE ID = 307");
    //renaming skill outfit recipes to new skill groups
    dbi.Update("UPDATE `recipe` SET  Name = 'Metalworking and Geology Outfit' WHERE ResultObjectTypeID = 303");
    dbi.Update("UPDATE `recipe` SET  Name = 'Carpentry Outfit' WHERE ResultObjectTypeID = 304");
    dbi.Update("UPDATE `recipe` SET  Name = 'Alchemy Outfit' WHERE ResultObjectTypeID = 305");
    dbi.Update("UPDATE `recipe` SET  Name = 'Building Outfit' WHERE ResultObjectTypeID = 306");
    dbi.Update("UPDATE `recipe` SET  Name = 'Farming Outfit' WHERE ResultObjectTypeID = 307");
    //giving skill outfits recipe descriptions
    dbi.Update("UPDATE `recipe` SET  Description = 'Raises Metalworking and Geology Skills' WHERE ResultObjectTypeID = 303");
    dbi.Update("UPDATE `recipe` SET  Description = 'Raises Carpentry Skills' WHERE ResultObjectTypeID = 304");
    dbi.Update("UPDATE `recipe` SET  Description = 'Raises Alchemy Skills' WHERE ResultObjectTypeID = 305");
    dbi.Update("UPDATE `recipe` SET  Description = 'Raises Building Skills' WHERE ResultObjectTypeID = 306");
    dbi.Update("UPDATE `recipe` SET  Description = 'Raises Farming Skills' WHERE ResultObjectTypeID = 307");
  }

  function GearBalance::ringstack() {
    //makes basic rings and amulets stackable
    dbi.Update("UPDATE `objects_types` SET  MaxStackSize = 10000 WHERE ID = 479");
    dbi.Update("UPDATE `objects_types` SET  MaxStackSize = 10000 WHERE ID = 487");
    dbi.Update("UPDATE `objects_types` SET  MaxStackSize = 10000 WHERE ID = 499");
    dbi.Update("UPDATE `objects_types` SET  MaxStackSize = 10000 WHERE ID = 488");
  }
  function GearBalance::gmCharacterTable() {
    dbi.Update("ALTER TABLE `character` ADD COLUMN `isGM` TINYINT UNSIGNED NULL DEFAULT NULL AFTER `WasInJudgmentHourOnLogout`;");
  }
  function GearBalance::gmCharacter(%this, %client)
  {
    dbi.Select(GearBalance, "setGM", "SELECT isGM, " @ %client @ " as client from `character` where ID = " @ %client.Player.getCharacterId()); 
  }
  function GearBalance::setGM(%this, %resultSet)
  {
    if(%resultSet.ok() && %resultSet.nextRecord()) {
      %isGM = %resultSet.getFieldValue("isGM");
      if(%isGM)
      {
        %client = %resultSet.getFieldValue("client");
        %client.Player.setGM(true);
      }
    }
    %resultSet.delete();
  }
};
activatePackage(GearBalance);
LiFx::registerCallback($LiFx::hooks::mods, setup, GearBalance);
