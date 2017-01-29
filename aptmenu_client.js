
//todo : clean shit up, probably use enums or something

menuPool = API.getMenuPool();

//Begin main menu -----------------------------------------------------------------------

let aptNames = ["Modern Apartment", "Moody Apartment", "Vibrant Apartment",
                "Sharp Apartment", "Monochrome Apartment", "Seductive Apartment",
                "Regal Apartment", "Aqua Apartment"];

function setupMenu(aptNames) {
  let menu = API.createMenu("Apartments", 0, 0, 6);
  let floor1Menu = API.createMenu("Floor 1", 0, 0, 6);
  let floor2Menu = API.createMenu("Floor 2", 0, 0, 6);
  let floor3Menu = API.createMenu("Floor 3", 0, 0, 6);

  let sub1 = API.createMenuItem("Floor 1", "");
  let sub2 = API.createMenuItem("Floor 2", "");
  let sub3 = API.createMenuItem("Floor 3", "");


  menu.AddItem(sub1);
  menu.AddItem(sub2);
  menu.AddItem(sub3);

  // Loop through all the names.
  aptNames.forEach(function (name) {
      tempItem1 = API.createMenuItem(item, "Floor 1"));
      tempItem2 = API.createMenuItem(item, "Floor 2"));
      tempItem3 = API.createMenuItem(item, "Floor 3"));
      floor1Menu.AddItem(tempItem1);
      floor2Menu.AddItem(tempItem2);
      floor3Menu.AddItem(tempItem3);
    }

    menuPool.Add(menu);
    menuPool.Add(menu1);
    menuPool.Add(menu2);
    menuPool.Add(menu3);

}


setupMenu(aptNames);
let keyDetect = false;

//End main menu -----------------------------------------------------------------------

menu1.OnItemSelect.connect(function(sender, item, index) { //Prefix explanation -- 1st #:Floor number; 2nd and 3rd #: Apt number
    switch (item) {
        case item1:
            API.triggerServerEvent("loadApt101");
            break;
        case item4:
            API.triggerServerEvent("loadApt102");
            break;
        case item7:
            API.triggerServerEvent("loadApt103");
            break;
        case item10:
            API.triggerServerEvent("loadApt104");
            break;
        case item13:
            API.triggerServerEvent("loadApt105");
            break;
        case item16:
            API.triggerServerEvent("loadApt106");
            break;
        case item19:
            API.triggerServerEvent("loadApt107");
            break;
        case item22:
            API.triggerServerEvent("loadApt108");
            break;
    }
    menu1.Visible = false;
});

menu2.OnItemSelect.connect(function (sender, item, index) { //Prefix explanation -- 1st #:Floor number; 2nd and 3rd #: Apt number
    switch (item) {
        case item2:
            API.triggerServerEvent("loadApt201");
            break;
        case item5:
            API.triggerServerEvent("loadApt202");
            break;
        case item8:
            API.triggerServerEvent("loadApt203");
            break;
        case item11:
            API.triggerServerEvent("loadApt204");
            break;
        case item14:
            API.triggerServerEvent("loadApt205");
            break;
        case item17:
            API.triggerServerEvent("loadApt206");
            break;
        case item20:
            API.triggerServerEvent("loadApt207");
            break;
        case item23:
            API.triggerServerEvent("loadApt208");
            break;
    }
    menu2.Visible = false;
});

menu3.OnItemSelect.connect(function (sender, item, index) { //Prefix explanation -- 1st #:Floor number; 2nd and 3rd #: Apt number
    switch (item) {
        case item3:
            API.triggerServerEvent("loadApt301");
            break;
        case item6:
            API.triggerServerEvent("loadApt302");
            break;
        case item9:
            API.triggerServerEvent("loadApt303");
            break;
        case item12:
            API.triggerServerEvent("loadApt304");
            break;
        case item15:
            API.triggerServerEvent("loadApt305");
            break;
        case item18:
            API.triggerServerEvent("loadApt306");
            break;
        case item21:
            API.triggerServerEvent("loadApt307");
            break;
        case item24:
            API.triggerServerEvent("loadApt308");
            break;
    }
    menu3.Visible = false;
});

menu.OnItemSelect.connect(function (sender, item, index) {
    switch (item) {
        case sub1:
            menu.Visible = false;
            menu1.Visible = true;
            break;
        case sub2:
            menu.Visible = false;
            menu2.Visible = true;
            break;
        case sub3:
            menu.Visible = false;
            menu3.Visible = true;
            break;
    }
    API.showCursor(false);
    menu.Visible = false;
});

API.onKeyDown.connect(function (sender, e) {
    if (keyDetect == true) {
        if (e.KeyCode == Keys.E) {
            menu.Visible = true;
            menu1.Visible = false;
            menu2.Visible = false;
            menu3.Visible = false;
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
            menu1.Visible = false;
            menu2.Visible = false;
            menu3.Visible = false;

            break;
    }
});

API.onUpdate.connect(function (sender, events) {
    menuPool.ProcessMenus();
});
