using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;

//test

public class apartment_menu : Script
{
    //Variable declaration
    string[] fl01_List = new string[]
    {
        null, "apa_v_mp_h_01_a", "apa_v_mp_h_02_a", "apa_v_mp_h_03_a", "apa_v_mp_h_04_a", "apa_v_mp_h_05_a", "apa_v_mp_h_06_a", "apa_v_mp_h_07_a", "apa_v_mp_h_08_a"
    };

    string[] fl02_List = new string[]
    {
        null, "apa_v_mp_h_01_c", "apa_v_mp_h_02_c", "apa_v_mp_h_03_c", "apa_v_mp_h_04_c", "apa_v_mp_h_05_c", "apa_v_mp_h_06_c", "apa_v_mp_h_07_c", "apa_v_mp_h_08_c"
    };

    string[] fl03_List = new string[]
    {
        null, "apa_v_mp_h_01_b", "apa_v_mp_h_02_b", "apa_v_mp_h_03_b", "apa_v_mp_h_04_b", "apa_v_mp_h_05_b", "apa_v_mp_h_06_b", "apa_v_mp_h_07_b", "apa_v_mp_h_08_b"
    };

    int[] occupation = new int[]
    {
        0, 0, 0, 0  //Amount of players in each floor. Incriments with players entering floors
    };

    int[] currFloor = new int[]
    {
        0, 0, 0, 0
    };

    Vector3 frontDoor = new Vector3(-773.915, 311.579, 84.698);
    Vector3 apa1 = new Vector3(-786.8663, 315.7642, 216.6385); //1
    Vector3 apa2 = new Vector3(-786.9563, 315.6229, 186.9136); //2
    Vector3 apa3 = new Vector3(-774.0126, 342.0428, 195.6864);//3
                                                            
    //End variable declaration -------------------------------------------------------------------------------------------------------------------------------------------

    [Command("tpapart")] //debug TP command
    public void tp(Client sender)
    {
        sender.position = new Vector3(-773.915, 311.579, 84.698);
    }

    public apartment_menu()
    {
        var apt_colShape = API.createCylinderColShape(frontDoor, 2, 2); //Create Initial Collision shape
        var fl1_colShape = API.createCylinderColShape(apa1, 1f, 2); //floor 1 col shape
        var fl2_colShape = API.createCylinderColShape(apa2, 1f, 2); //floor 2 col shape
        var fl3_colShape = API.createCylinderColShape(apa3, 1f, 2); //floor 3 col shape

        apt_colShape.onEntityEnterColShape += apt_enterColShape; //Enter init col shape
        apt_colShape.onEntityExitColShape += apt_exitColShape; //Exit init col shape

        fl1_colShape.onEntityEnterColShape += fl1_entercolShape; //floor 1 col shape
        fl2_colShape.onEntityEnterColShape += fl2_enterColShape;
        fl3_colShape.onEntityEnterColShape += fl3_enterColShape;


        API.onClientEventTrigger += OnClientEvent;
        createMarkers(); //create all markers
    }//main constructor

    public void createMarkers()
    {
        API.createMarker(1, frontDoor, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //init marker
        API.createMarker(1, apa1, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //floor 1
        API.createMarker(1, apa2, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //floor 2
        API.createMarker(1, apa3, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //floor 3
    }//create all markers

    public void apt_enterColShape(ColShape shape, NetHandle Entity) //enter init shape -- trigger JS create menu
    {
        var player = API.getPlayerFromHandle(Entity);
        if (player == null)
        {
            return;
        }
        API.sendNotificationToPlayer(player, "Press E to open the apartment menu.");
        API.triggerClientEvent(player, "createAptMenu");
    }

    public void apt_exitColShape(ColShape shape, NetHandle Entity)
    {
        var player = API.getPlayerFromHandle(Entity);
        if (player == null)
        {
            return;
        }
        API.triggerClientEvent(player, "destroyAptMenu");
    } //exit init col shape

    public void OnClientEvent(Client player, string eventName, params object[] arguments) //player selected menu item triggers
    {
        string aptFlr = null;
        string aptStr = null;
        int aptInt = 0;

        aptFlr = eventName.Substring(7, 1); //Floor number of the apartment
        aptStr = eventName.Substring(8, 2); //Apartment number (0-8 per floor)
        aptInt = Convert.ToInt32(aptStr); //Apt Number ToString

        if (aptFlr == "1")
        {
            if (occupation[1] == 0) // FLOOR 1
            {
                API.requestIpl(fl01_List[Convert.ToInt32(aptStr)]); //Request a floor number using the index of fl01_list
                currFloor[1] = aptInt;
                player.position = apa1 + new Vector3(5, 0, 0);
                API.sendNotificationToPlayer(player, "Floor 1");
                occupation[1]++; //add a player to the occupation
            }
            else if (occupation[1] > 0)
            {
                if (aptInt == currFloor[1])
                {
                    API.sendNotificationToPlayer(player, "Floor 1 Occupied.");
                    API.sendNotificationToPlayer(player, "Joining active room...");
                    player.position = apa1;
                    occupation[1]++;
                }
                else
                {
                    API.sendNotificationToPlayer(player, "That floor is occupied!");
                }
            }
        }
        if (aptFlr == "2")
        {
            if (occupation[2] == 0) // FLOOR 2
            {
                API.requestIpl(fl02_List[Convert.ToInt32(aptStr)]); //Request a floor number using the index of fl02_list
                currFloor[2] = aptInt;
                player.position = apa2 + new Vector3(5, 0, 0);
                API.sendNotificationToPlayer(player, "Floor 2");
                occupation[2]++; //add a player to the occupation
            }
            else if (occupation[2] > 0)
            {
                if (aptInt == currFloor[2])
                {
                    API.sendNotificationToPlayer(player, "Floor 2 Occupied.");
                    API.sendNotificationToPlayer(player, "Joining active room...");
                    player.position = apa2;
                    occupation[1]++;
                }
                else
                {
                    API.sendNotificationToPlayer(player, "That floor is occupied!");
                }
            }
        }
        if (aptFlr == "3")
        {
            if (occupation[3] == 0) // FLOOR 3
            {
                API.requestIpl(fl03_List[Convert.ToInt32(aptStr)]); //Request a floor number using the index of fl03_list
                currFloor[3] = aptInt;
                player.position = apa3 + new Vector3(-5, 0, 0);
                API.sendNotificationToPlayer(player, "Floor 3");
                occupation[3]++; //add a player to the occupation
            }
            else if (occupation[3] > 0)
            {
                if (aptInt == currFloor[3])
                {
                    API.sendNotificationToPlayer(player, "Floor 3 Occupied.");
                    API.sendNotificationToPlayer(player, "Joining active room...");
                    player.position = apa3;
                    occupation[3]++;
                }
                else
                {
                    API.sendNotificationToPlayer(player, "That floor is occupied!");
                }
            }
        }
    }

    public void fl1_entercolShape(ColShape shape, NetHandle Entity)
    {
        API.setEntityPosition(Entity, frontDoor + new Vector3(0, -10, 0));
        occupation[1]--;
        if (occupation[1] == 0)
        {
            for (var i = 0; i < fl01_List.Length; i++)
            {
                var item = fl01_List[i];
                API.removeIpl(item);
            }
            currFloor[1] = 0;
        }
    } //floor 1

    public void fl2_enterColShape(ColShape shape, NetHandle Entity)
    {
        API.setEntityPosition(Entity, frontDoor + new Vector3(0, -10, 0));
        occupation[2]--;
        if (occupation[2] == 0)
        {
            for (var i = 0; i < fl02_List.Length; i++)
            {
                var item = fl02_List[i];
                API.removeIpl(item);
            }
            currFloor[2] = 0;
        }
    } //floor 2

    public void fl3_enterColShape(ColShape shape, NetHandle Entity)
    {
        API.setEntityPosition(Entity, frontDoor + new Vector3(0, -10, 0));
        occupation[3]--;
        if (occupation[3] == 0)
        {
            for (var i = 0; i < fl03_List.Length; i++)
            {
                var item = fl03_List[i];
                API.removeIpl(item);
            }
            currFloor[3] = 0;
        }
    } //floor 3
}