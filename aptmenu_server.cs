using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;

//Todo: Function for unloading apartments.

public class apartment_menu : Script
{

    string[,] aptArray = new string[,] //fuck you, we're going multi-dimensional, man!
    {
        {"", "", "", "", "", "", "", "", "", ""}, //floor 0
        {"", "apa_v_mp_h_01_a", "apa_v_mp_h_02_a", "apa_v_mp_h_03_a", "apa_v_mp_h_04_a", "apa_v_mp_h_05_a", "apa_v_mp_h_06_a", "apa_v_mp_h_07_a", "apa_v_mp_h_08_a", ""}, //floor 1
        {"", "apa_v_mp_h_01_c", "apa_v_mp_h_02_c", "apa_v_mp_h_03_c", "apa_v_mp_h_04_c", "apa_v_mp_h_05_c", "apa_v_mp_h_06_c", "apa_v_mp_h_07_c", "apa_v_mp_h_08_c", ""}, //floor 2
        {"", "apa_v_mp_h_01_b", "apa_v_mp_h_02_b", "apa_v_mp_h_03_b", "apa_v_mp_h_04_b", "apa_v_mp_h_05_b", "apa_v_mp_h_06_b", "apa_v_mp_h_07_b", "apa_v_mp_h_08_b", ""}, //floor 3
        {"", "ex_dt1_02_office_02b", "ex_dt1_02_office_02c", "ex_dt1_02_office_02a", "ex_dt1_02_office_01a", "ex_dt1_02_office_01b", "ex_dt1_02_office_01c", "ex_dt1_02_office_03a", "ex_dt1_02_office_03b", "ex_dt1_02_office_03c"}, //floor 3
    };

    Vector3[] aptPos = new Vector3[]
    {
        new Vector3(0, 0, 0), new Vector3(-786.8663, 315.7642, 216.6385), new Vector3(-786.9563, 315.6229, 186.9136), new Vector3(-774.0126, 342.0428, 195.6864), new Vector3(-141.19, -620.91, 168.82)
    };

    int[] occupation = new int[]
    {
        0, 0, 0, 0, 0, 0, 0  //Amount of players in each floor. Incriments with players entering floors.
    };

    int[] currAptIndex = new int[]
    {
        0, 0, 0, 0, 0, 0, 0 //Index of the current in-use apartment
    };

    Vector3 frontDoor = new Vector3(-773.915, 311.579, 84.698);


                                                            
    //End variable declaration -------------------------------------------------------------------------------------------------------------------------------------------

    [Command("tpapart")] //debug TP command
    public void tp(Client sender)
    {
        sender.position = new Vector3(-773.915, 311.579, 84.698);
    }

    public apartment_menu()
    {
        var apt_colShape = API.createCylinderColShape(frontDoor, 2, 2); //Create Initial Collision shape
        var fl1_colShape = API.createCylinderColShape(aptPos[1], 1f, 2); //floor 1 col shape
        var fl2_colShape = API.createCylinderColShape(aptPos[2], 1f, 2); //floor 2 col shape
        var fl3_colShape = API.createCylinderColShape(aptPos[3], 1f, 2); //floor 3 col shape
        var fl4_colShape = API.createCylinderColShape(aptPos[3], 1f, 2);
        apt_colShape.onEntityEnterColShape += apt_enterColShape; //Enter init col shape
        apt_colShape.onEntityExitColShape += apt_exitColShape; //Exit init col shape
        fl1_colShape.onEntityEnterColShape += fl1_entercolShape; //floor 1 col shape
        fl2_colShape.onEntityEnterColShape += fl2_enterColShape;
        fl3_colShape.onEntityEnterColShape += fl3_enterColShape;
        //fl4_colShape.onEntityEnterColShape += fl4_enterColShape;
        API.onClientEventTrigger += OnClientEvent;
        createMarkers(); //create all markers
    }//main constructor

    public void createMarkers()
    {
        API.createMarker(1, frontDoor, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //init marker
        API.createMarker(1, aptPos[1], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //floor 1
        API.createMarker(1, aptPos[2], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //floor 2
        API.createMarker(1, aptPos[3], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //floor 3
        API.createMarker(1, aptPos[4], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), 100, 255, 255, 0, 0); //Arcadius Building
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

    public void joinRoom(Client player, string eventName) //sheeeiiittt
    {
        string aptFlr = eventName.Substring(7, 1); //Floor number of the apartment
        string aptStr = eventName.Substring(8, 2); //Apartment number (0-8 per floor)
        int aptInt = Convert.ToInt32(aptStr); //Apt Number ToString
        int aptFlrInt = Convert.ToInt32(aptFlr); //Apt floor ToString

        if (occupation[aptFlrInt] == 0) //if nobody is inside the floor...
        {
            currAptIndex[aptFlrInt] = aptInt; //Set the Apartment index to the int of the apt. (If using apt 6 on floor 1, set  aptFlrInt[1] = 06)
            API.sendNotificationToPlayer(player, String.Format("Floor {0}, Apartment #:{1}", aptFlr, aptStr) );
            occupation[aptFlrInt]++; //incriment occupation
            API.requestIpl(aptArray[aptFlrInt, aptInt]);
            if (aptFlrInt == 3) //Failsafe -- floor 3 is facing the wrong direction 
            {
                Thread.Sleep(200);
                player.position = aptPos[aptFlrInt] + new Vector3(-2, 0, 0);
            }
            else
            {
                Thread.Sleep(200);
                player.position = aptPos[aptFlrInt] + new Vector3(2, 0, 0);
            }
        }
        else if (occupation[aptFlrInt] > 0)
        {
            if (aptInt == currAptIndex[aptFlrInt])
            {
                API.sendNotificationToPlayer(player, "Floor Occupied.");
                API.sendNotificationToPlayer(player, "Joining active room...");
                if (aptFlrInt == 3) //Failsafe -- floor 3 is facing the wrong direction 
                {
                    Thread.Sleep(200);
                    player.position = aptPos[aptFlrInt] + new Vector3(-2, 0, 0);
                }
                else
                {
                    Thread.Sleep(200);
                    player.position = aptPos[aptFlrInt] + new Vector3(2, 0, 0);
                }
                occupation[aptFlrInt]++;
            }
            else
            {
                API.sendNotificationToPlayer(player, "That floor is occupied!");
            }
        }

    }

    public void leaveRoom(int flN, NetHandle Entity)
    {
        Thread.Sleep(200);
        API.setEntityPosition(Entity, frontDoor + new Vector3(0, -10, 0));
        occupation[flN]--;
        if (occupation[flN] == 0)
        {
            for (var i = 0; i < 9; i++)
            {
                var item = aptArray[flN, i];
                API.removeIpl(item);
                currAptIndex[flN] = 0;
            }
        }
    }

    public void OnClientEvent(Client player, string eventName, params object[] arguments) //player selected menu item triggers
    {
        joinRoom(player, eventName);
    }

    public void fl1_entercolShape(ColShape shape, NetHandle Entity)
    {
        leaveRoom(1, Entity);
    } //floor 1

    public void fl2_enterColShape(ColShape shape, NetHandle Entity)
    {
        leaveRoom(2, Entity);
    } //floor 1

    public void fl3_enterColShape(ColShape shape, NetHandle Entity)
    {
        leaveRoom(3, Entity);
    } //floor 1
}