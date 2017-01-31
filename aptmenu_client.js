
//todo : clean shit up, probably use enums or something

// Debug option
let debugging = true;

//Begin main menu -----------------------------------------------------------------------

let menuPool = null;

let floorMenus = [];
let aptMenu = null;
//Prefix explanation -- 1st #:Floor number; 2nd and 3rd #: Apt number
let aptNames =  [
        {
            "name": "Modern Apartment",
            "floorNum": 3, 
            "floorData": [
                "loadApt101",
                "loadApt201",
                "loadApt301"
            ],
        },
        {
            "name": "Moody Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt102",
                "loadApt202",
                "loadApt302",
            ],
        },
        {
            "name": "Vibrant Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt103",
                "loadApt203",
                "loadApt303",
            ],
        },
        {
            "name": "Sharp Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt104",
                "loadApt204",
                "loadApt304",
            ],
        },
        {
            "name": "Monochrome Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt105",
                "loadApt205",
                "loadApt305",
            ],
        },
        {
            "name": "Seductive Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt106",
                "loadApt206",
                "loadApt306",
            ],
        },
        {
            "name": "Regal Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt107",
                "loadApt207",
                "loadApt307",
            ],
        },
        {
            "name": "Aqua Apartment",
            "floorNum": 3,
            "floorData": [
                "loadApt108",
                "loadApt208",
                "loadApt308",
            ],
        },
        {
            "name": "Executive Rich",
            "floorNum": 9,
            "floorData": [
                "loadApt401",
                "loadApt402",
                "loadApt403",
                "loadApt404",
                "loadApt405",
                "loadApt406",
                "loadApt407",
                "loadApt408",
                "loadApt409",
            ],
        }
    ]

setupMenus(aptNames);
  
function setupMenus(aptNames) {
    menuPool = API.getMenuPool();
    
    floorMenus = createFloorMenus(aptNames);

    floorMenus.forEach(function (subMenu) {
        menuPool.Add(subMenu);
    });

    aptMenu = createAptMenu(aptNames, floorMenus);
    menuPool.Add(aptMenu);
}

function createFloorMenus(aptNames) {

    let floorMenus = [];

    // Loop through all the apartments
    aptNames.forEach(function (apt) {
        // Create the floor menus of apartments.

        API.sendChatMessage(apt.name);
        tempFloorMenu = API.createMenu(apt.name, "Floor Selection", 0, 0, 6);

        for (let i = 0; i < apt.floorData.length; i++) {
            floorMenuItem = API.createMenuItem("Floor " + (i + 1), "");

            // Assign load function to the item.
            floorMenuItem.Activated.connect(function (menu, item) {
                API.triggerServerEvent(apt.floorData[i]);

                menu.Visible = false;
            });

            tempFloorMenu.AddItem(floorMenuItem);
        }

        floorMenus.push(tempFloorMenu);
    });

    return floorMenus;
}

function createAptMenu(aptNames, floorMenus) {
    let menu = API.createMenu("Apartments", 0, 0, 6);

    let tempMenuIndex = 0;

    // Loop through all the apartments
    for(let i = 0; i < aptNames.length; i++) {
        // Create the main menu of apartments.

        mainMenuNameItem = API.createMenuItem(aptNames[i].name, "");

        // Assign floor menu to each apartment menu item.
        mainMenuNameItem.Activated.connect(function (menu, item) {
            // opens floor menu 
            API.sendChatMessage(i.toString());

            menu.Visible = false;

            floorMenus[i].Visible = true;
        });

        tempMenuIndex++;
        menu.AddItem(mainMenuNameItem);

    }

    return menu;
}

//End main menu -----------------------------------------------------------------------


API.onKeyDown.connect(function (sender, e) {
    if (debugging) {
        if (!aptMenu.Visible && e.KeyCode == Keys.E && !API.isChatOpen()) {
            aptMenu.Visible = true;

            floorMenus.forEach(function (subMenu) {
                subMenu.Visible = false;
            });

            API.showCursor(false);
        } else if (e.KeyCode == Keys.E && !API.isChatOpen()){
            aptMenu.Visible = false;

            floorMenus.forEach(function (subMenu) {
                subMenu.Visible = false;
            });

            API.showCursor(false);
        }
    }
});

API.onServerEventTrigger.connect(function (eventName, args) { //detects triggers like tumblr
    switch (eventName) {

        case 'createAptMenu':
            keyDetect = true;
            break;


        case 'destroyAptMenu':
            keyDetect = false;
            API.setHudVisible(true);
            menu.Visible = false;
            floor1Menu.Visible = false;
            floor2Menu.Visible = false;
            floor3Menu.Visible = false;
            executiveMenu.Visible = false;

            break;
    }
});

API.onUpdate.connect(function (sender, events) {
    menuPool.ProcessMenus();
});
        