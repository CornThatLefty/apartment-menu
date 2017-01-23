
//todo : clean shit up, probably use enums or something

menuPool = API.getMenuPool();

//Begin main menu -----------------------------------------------------------------------
let menu = API.createMenu("Apartments", 0, 0, 6);
let menu1 = API.createMenu("Floor 1", 0, 0, 6);
let menu2 = API.createMenu("Floor 2", 0, 0, 6);
let menu3 = API.createMenu("Floor 3", 0, 0, 6);

let sub1 = API.createMenuItem("Floor 1", "");
let sub2 = API.createMenuItem("Floor 2", "");
let sub3 = API.createMenuItem("Floor 3", "");

let item1 = API.createMenuItem("Modern Apartment", "Floor 1");
let item2 = API.createMenuItem("Modern Apartment", "Floor 2");
let item3 = API.createMenuItem("Modern Apartment", "Floor 3");
let item4 = API.createMenuItem("Moody Apartment", "Floor 1");
let item5 = API.createMenuItem("Moody Apartment", "Floor 2");
let item6 = API.createMenuItem("Moody Apartment", "Floor 3");
let item7 = API.createMenuItem("Vibrant Apartment", "Floor 1");
let item8 = API.createMenuItem("Vibrant Apartment", "Floor 2");

let item9 = API.createMenuItem("Vibrant Apartment", "Floor 3");
let item10 = API.createMenuItem("Sharp Apartment", "Floor 1");
let item11 = API.createMenuItem("Sharp Apartment", "Floor 2");
let item12 = API.createMenuItem("Sharp Apartment", "Floor 3");
let item13 = API.createMenuItem("Monochrome Apartment", "Floor 1");
let item14 = API.createMenuItem("Monochrome Apartment", "Floor 2");
let item15 = API.createMenuItem("Monochrome Apartment", "Floor 3");
let item16 = API.createMenuItem("Seductive Apartment", "Floor 1");

let item17 = API.createMenuItem("Seductive Apartment", "Floor 2");
let item18 = API.createMenuItem("Seductive Apartment", "Floor 3");
let item19 = API.createMenuItem("Regal Apartment", "Floor 1");
let item20 = API.createMenuItem("Regal Apartment", "Floor 2");
let item21 = API.createMenuItem("Regal Apartment", "Floor 3");
let item22 = API.createMenuItem("Aqua Apartment ", "Floor 1");
let item23 = API.createMenuItem("Aqua Apartment ", "Floor 2");
let item24 = API.createMenuItem("Aqua Apartment ", "Floor 3");

menu.AddItem(sub1);
menu.AddItem(sub2);
menu.AddItem(sub3);

menu1.AddItem(item1);
menu2.AddItem(item2);
menu3.AddItem(item3);
menu1.AddItem(item4);
menu2.AddItem(item5);
menu3.AddItem(item6);
menu1.AddItem(item7);
menu2.AddItem(item8);
menu3.AddItem(item9);
menu1.AddItem(item10);
menu2.AddItem(item11);
menu3.AddItem(item12);
menu1.AddItem(item13);
menu2.AddItem(item14);
menu3.AddItem(item15);
menu1.AddItem(item16);
menu2.AddItem(item17);
menu3.AddItem(item18);
menu1.AddItem(item19);
menu2.AddItem(item20);
menu3.AddItem(item21);
menu1.AddItem(item22);
menu2.AddItem(item23);
menu3.AddItem(item24);


let keyDetect = false;

//End main menu -----------------------------------------------------------------------

menuPool.Add(menu);
menuPool.Add(menu1);
menuPool.Add(menu2);
menuPool.Add(menu3);

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