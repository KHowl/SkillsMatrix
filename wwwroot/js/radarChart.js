window.chartColors = {
    red: 'rgb(255, 99, 132)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)'
};

window.gradient = {
        0:   [0, 0, 0, 1],       // Black
        15:  [70, 70, 0, 1],     // Dark Mustard
        40:  [220, 0, 0, 1],     // Dark Red 
        70:  [80, 80, 255, 1],   // Blue
        100: [30, 220, 30, 1]    // Green
    };

window.max_skills = 30;

// Sort difference function for strings
function ObjectSortString(property) {
    var sortOrder = 1;

    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }

    return function (a, b) {
        ap = String(a[property]);
        bp = String(b[property]);

        if (sortOrder == -1) {
            return bp.localeCompare(ap);
        } else {
            return ap.localeCompare(bp);
        }
    }
}

// Sort difference function for numbers
function ObjectSortNumber(property) {
    var sortOrder = 1;

    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }

    return function (a, b) {
        if (sortOrder == -1) {
            return +b[property] - +a[property];
        } else {
            return +a[property] - +b[property];
        }
    }
}

// Return array of string values, or NULL if CSV string not well formed.
function CSVtoArray(text) {
    var re_valid = /^\s*(?:'[^'\\]*(?:\\[\S\s][^'\\]*)*'|"[^"\\]*(?:\\[\S\s][^"\\]*)*"|[^,'"\s\\]*(?:\s+[^,'"\s\\]+)*)\s*(?:,\s*(?:'[^'\\]*(?:\\[\S\s][^'\\]*)*'|"[^"\\]*(?:\\[\S\s][^"\\]*)*"|[^,'"\s\\]*(?:\s+[^,'"\s\\]+)*)\s*)*$/;
    var re_value = /(?!\s*$)\s*(?:'([^'\\]*(?:\\[\S\s][^'\\]*)*)'|"([^"\\]*(?:\\[\S\s][^"\\]*)*)"|([^,'"\s\\]*(?:\s+[^,'"\s\\]+)*))\s*(?:,|$)/g;
    // Return NULL if input string is not well formed CSV string.
    if (!re_valid.test(text)) return null;
    var a = [];                     // Initialize array to receive values.
    text.replace(re_value, // "Walk" the string using replace with callback.
        function (m0, m1, m2, m3) {
            // Remove backslash from \' in single quoted values.
            if (m1 !== undefined) a.push(m1.replace(/\\'/g, "'"));
            // Remove backslash from \" in double quoted values.
            else if (m2 !== undefined) a.push(m2.replace(/\\"/g, '"'));
            else if (m3 !== undefined) a.push(m3);
            return ''; // Return empty string.
        });
    // Handle special case of empty last value.
    if (/,\s*$/.test(text)) a.push('');
    return a;
};

// All SABA skill levels so far found?
window.skill_types = [ 
        ['Conceptual/Trained', 'Experienced', 'Expert', 'Guru', ''],
        ['Elementary', 'Limited Working', 'Professional Working', 'Full Professional', 'Native or bilingual'],
        ['','','','Certified',''],
        ['','Tertiary', 'Secondary', 'Primary', ''],
        ['', 'Skilled', 'Talented', 'Towering Strength', '']
    ];

// Function to convert SABA skills to numerical levels
function ConvertToLevel(str) {
    level = 0;
    if (str != '')
    {
        for (var i = 0; i < window.skill_types.length; i++) {
            for (var j = 0; j < window.skill_types[i].length; j++)
            {
                if (str == window.skill_types[i][j]) {
                    level = j+1;
                    break;
                }
            }
        }
    }

    return level;
}

// Find which type of Skill level is being used
function GetSkillType(held) {
    // Find skill level type in array
    var skill_type = 0;
    for (var k = 0; k < window.skill_types.length; k++) {
        for (var l = 0; l < window.skill_types[k].length; l++)
        {
            if (window.skill_types[k][l] == held) {
                skill_type = k;
                break;
            }
        }
    }

    return skill_type;
}

// Find the index of a named property value in an array of objects.
function FindPropertyIndex(array, property, value) {
    if (array.findIndex) {
        return array.findIndex(function(obj){ return obj && obj[property] == value });
    } else {
        // IE11
        var result = -1;
        for (var i = 0; i < array.length; i++) {
            if (array[i] && array[i][property] == value) {
                result = i;
                break;
            }
        }
        return result;
    }
}

// Find the skill index for a specific skill name
function FindSkillIndexByName(skillName) {
    return FindPropertyIndex(window.all_skills, "name", skillName);
}

// Find the person index for a specific person's name
function FindPersonIndexByName(person_name) {
    return FindPropertyIndex(window.people, "name", person_name);
}

// Populate the SKills store independently as these will need updating if manager or location is selected.
function PopulateSkillsStore(manager, location) {
    // Populate list of all skills
    window.all_skills = []
    for (var i = 0; i < window.people.length; i++) {

        if ((location == 'Any' || location == window.people[i].location) &&
            (manager == 'Any' || manager == window.people[i].manager)) {
            skills = window.people[i].skills;

            for (var j = 0; j < skills.length; j++) {
                var skill_index = FindSkillIndexByName(skills[j].name);

                if (skill_index != -1) {
                    // Update existing skill name totals
                    var skill = window.all_skills[skill_index];

                    skill.total++;
                    for (var k = 0; k < 6; k++) {
                        if (skills[j].heldLevel == k) { skill.levelTotal[k]++; }
                    }

                } else {

                    // Find skill level type in array
                    var skill_type = GetSkillType(skills[j].held);

                    // Create a new skill name entry, totals will be updated when we find others
                    var skill = { 
                        name: skills[j].name, 
                        group: skills[j].group,
                        parent: skills[j].parent,
                        type: skill_type,
                        total: 1, 
                        levelTotal: []
                    };

                    for (var k = 0; k < 6; k++) {
                        skill.levelTotal[k] = skills[j].heldLevel == k ? 1 : 0;
                    }


                    window.all_skills.push(skill);
                }
            }
        }
    }

    // Sort by name (increasing)
    window.all_skills.sort(ObjectSortString("name"));

    // Populate list of all skill groups
    window.all_groups = [];
    for (var i = 0; i < window.people.length; i++) {
        if ((location == 'Any' || location == window.people[i].location) &&
            (manager == 'Any' || manager == window.people[i].manager)) {

            skills = window.people[i].skills;

            for (var j = 0; j < skills.length; j++) {
                if (window.all_groups.indexOf(skills[j].group) == -1) {
                    window.all_groups.push(skills[j].group);
                }
            }
        }
    }

    // Sort skill groups alphabetically
    window.all_groups.sort();

    // Sub-list of groups for when a parent group is selected, created on the fly
    window.selected_groups = [];

    // Populate list of all skill parent groups
    window.all_parents = [];
    for (var i = 0; i < window.people.length; i++) {
        if ((location == 'Any' || location == window.people[i].location) &&
            (manager == 'Any' || manager == window.people[i].manager)) {
            skills = window.people[i].skills;

            for (var j = 0; j < skills.length; j++) {
                if (window.all_parents.indexOf(skills[j].parent) == -1) {
                    window.all_parents.push(skills[j].parent);
                }
            }
        }
    }

    // Sort skill parent groups alphabetically
    window.all_parents.sort();
}

// Function to import csv file into local storage
function PopulateStore(rows) {
    // Populate what we already have
    window.headers = CSVtoArray(rows[0]);
    window.managers = [];
    window.locations = [];
    window.people = [];

    // Process the rest of the rows
    for (var i = 1; i < rows.length; i++) {
        var cells = CSVtoArray(rows[i]);
        if (cells.length > 1) {
            var manager = Math.random() >= 0.5 ? cells[0].trim() : 'Rob Gardner';
            var personLocation = Math.random() >= 0.5 ? 'Fleet' : 'Basingstoke';

            // Populate managers list, create if not present
            var manager_index = FindPropertyIndex(window.managers, "name", manager);

            if (manager_index == -1) {
                window.managers.push({ name: manager, locations: [ personLocation ]});
            } else {
                if (window.managers[manager_index].locations.indexOf(personLocation) == -1) {
                    window.managers[manager_index].locations.push(personLocation);
                }
            }

            // Populate locations list, create if not present
            if (window.locations.indexOf(personLocation) == -1) {
                window.locations.push(personLocation);
            }

            var personName = cells[1].trim();
            var personNo = cells[2].trim();
            var personEmail = cells[3].trim();

            var skillName = cells[4].trim();
            var skillGroup = cells[5].trim();
            var skillParent = cells[6].trim();
            var skillRequired = cells[7].trim();
            var skillHeld = cells[8].trim();
            

            // Create skill object
            var skill = {
                name: skillName,
                group: skillGroup,
                parent: skillParent,
                required: skillRequired,
                held: skillHeld,
                heldLevel: 0,
                requiredLevel: 0
            };

            // Find if person is existing  
            var personIndex = FindPersonIndexByName(personName)

            // Insert person into list if not found, 
            if (personIndex == -1) {
                // Create person object
                var person = { 
                    name: personName, 
                    no: personNo, 
                    email: personEmail, 
                    manager: manager,
                    location: personLocation,
                    skills: [] 
                };
                person.skills.push(skill);
                window.people.push(person);
            } else {
                // else add skill to existing person
                window.people[personIndex].skills.push(skill);
            }
            
            person = undefined;
            skill = undefined;
        }
    }

    // Convert skill levels to level numbers and sort by level
    for (var i = 0; i < window.people.length; i++) {
        skills = window.people[i].skills;

        for (var j = 0; j < skills.length; j++) {
            held = skills[j].held;
            required = skills[j].required;

            skills[j].heldLevel = ConvertToLevel(held);
            skills[j].requiredLevel = ConvertToLevel(required);
        }

        // Function to sort by held and then required
        var comparator = function(A, B) {
                var result = B.heldLevel - A.heldLevel;
                if (result == 0) {
                    result = B.requiredLevel - A.requiredLevel;
                }
                return result;
            }

        skills.sort(comparator);
        
    }

    PopulateSkillsStore('Any','Any')
}

// Function to add an option to a select
function AddOptionToSelect(sel, text, value) {
    var opt = document.createElement('option');
    opt.appendChild(document.createTextNode(text));
    opt.value = value;
    sel.appendChild(opt);
}

// Function to populate a select with options.
// property is for when array is of objects
// first is initial option with value -1
function PopulateOptionSelect(sel, array, property, first) {
    sel.options.length = 0;
    
    if (first) {
        AddOptionToSelect(sel, first, -1);
    }
    for (var i = 0; i < array.length; i++) {
        if (property) {
            AddOptionToSelect(sel, array[i][property], i);
        } else {
            AddOptionToSelect(sel, array[i], i);
        }
    }
}

// Retrieve a list of managers
function RetrieveManagers() {
    var managers = [];

    for (var i = 0; i < window.managers.length; i++) {
        managers.push(window.managers[i].name)
    }

    return managers;
}

// Retrieve a list of locations
function RetrieveLocations(manager_index) {
    var locations = [];

    if (manager_index == -1) {
        for (var i = 0; i < window.locations.length; i++) {
            locations.push(window.locations[i])
        }
    } else {
        locations = window.managers[manager_index].locations;
    }

    return locations;
}

// Retrieve list of skill parent groups used
function RetrieveSkillParentGroups() {
    var skill_parent_groups = [];
    
    for (var i = 0; i < window.all_parents.length; i++) {
        skill_parent_groups.push(window.all_parents[i])
    }

    return skill_parent_groups;
}

// Retrieve list of skill groups used
function RetrieveSkillGroups() {
    var skill_groups = [];
    
    for (var i = 0; i < window.all_groups.length; i++) {
        skill_groups.push(window.all_groups[i])
    }

    return skill_groups;
}

// Retrieve list of skill types used
function RetrieveSkills() {
    var skills = [];
    
    for (var i = 0; i < window.all_skills.length; i++) {
        skills.push(window.all_skills[i].name)
    }

    return skills;
}

// Retrieve aggregated list of skill types available
function RetrieveSkillTypes() {
    var skill_types = ['Any']; // value zero

    for (var i = 0; i < window.skill_types[0].length; i++) {
        skill_type = '';
        
        for (var j = 0; j < window.skill_types.length; j++) {
            if (window.skill_types[j][i] != '') {
                if (skill_type == '') {
                    skill_type = window.skill_types[j][i]
                } else {
                    skill_type = skill_type + " / " + window.skill_types[j][i]
                }
            }
        }

        skill_types.push(skill_type);
    }

    return skill_types;
}

// Retrieve list of selected skills and their totals
function RetrieveSkillTotals(parent, group, min_level) {
    var skill_list = [];

    for (var i = 0; i < window.all_skills.length ; i++) {

        if ((group == 'All' || window.all_skills[i].group == group) && 
            (parent == 'All' || window.all_skills[i].parent == parent)) {

            if (min_level == 0) {

                skill_list.push({ name: window.all_skills[i].name, total: window.all_skills[i].total });

            } else {

                total = 0;
                for (var j = min_level; j < window.skill_types[0].length; j++) {
                    total += window.all_skills[i].levelTotal[j];
                }
                if (total > 0) {
                    skill_list.push({ name: window.all_skills[i].name, total: total });
                }
            }
            
        }
    }

    skill_list.sort(ObjectSortNumber("total"));

    return skill_list;
}

// Function to return true if every element in array is true.
function AllTrue(a) {
    var result = true;
    for (var i = 0; i < a.length; i++) {
        if (!a[i]) {
            result = false;
            break;
        }
    }
    return result;
}

// Retrieve a list of people with selected skills.
function RetrievePeopleWithSkill(manager, location, skill_names) {

    var people_with_skill = [];

    if (skill_names[0] != '') {
        for (var i = 0; i < window.people.length; i++) {
            if ((location == 'Any' || location == window.people[i].location) &&
                (manager == 'Any' || manager == window.people[i].manager)) {

                var skills = window.people[i].skills;
                var skills_found = []; 
                var skills_held = []; 
                var skills_heldLevel = [];

                var j,k;
                for (k = 0; k < skill_names.length; k++) {
                    skills_found[k] = (skill_names[k] == '');
                    skills_held[k]= '';
                    skills_heldLevel[k] = -1;
                }

                for (j = 0; j < skills.length; j++) {
                    for (k = 0; k < skill_names.length; k++) {
                        if ((skill_names[k] != '') && (skills[j].name == skill_names[k])) {
                            skills_found[k] = true;
                            skills_held[k] = skills[j].held;
                            skills_heldLevel[k] = skills[j].heldLevel;
                        }
                    }

                    if (AllTrue(skills_found)) {
                        people_with_skill.push({
                                name: window.people[i].name, 
                                no: window.people[i].no, 
                                email: window.people[i].email, 
                                manager: window.people[i].manager,
                                location: window.people[i].location,
                                held: skills_held,
                                heldLevel: skills_heldLevel,
                            })
                        break;
                    }
                }
            }
        }
    }

    // Local function to sort up to three skills
    var comparator = function(A,B) {
        var result = 0;
        for (var i = 0; i < A.heldLevel.length; i++) {
            result += (B.heldLevel[i] - A.heldLevel[i]) * (1.00 + (A.heldLevel.length-i)/100.0);
        }
        return result;
    }

    people_with_skill.sort(comparator);

    return people_with_skill;
}

// Retrieve a list of people limited by manager and location
function RetrievePeople(manager, location) {
    var people = [];

    for (var i = 0; i < window.people.length; i++) {
        if ((location == 'Any' || location == window.people[i].location) &&
            (manager == 'Any' || manager == window.people[i].manager)) {
            people.push(window.people[i].name);
        }
    }

    return people;
}

// Populate the manager select options
function PopulateManagerOptions() {
    var managers = RetrieveManagers();
    
    var sel = document.getElementById('slManager');
    PopulateOptionSelect(sel, managers, null, "Any")
    sel.value = -1;
}

// Populate the location select options
function PopulateLocationOptions(manager_index) {
    var locations = RetrieveLocations(manager_index);
    
    var sel = document.getElementById('slLocation');
    PopulateOptionSelect(sel, locations, null, "Any")
    sel.value = -1;
}

// Add parent groups and skill levels to drop downs
function PopulateParentGroupSkillOptions() {

    var skill_parent_groups = RetrieveSkillParentGroups();
    var skill_groups = RetrieveSkillGroups();
    var skill_types = RetrieveSkillTypes();

    var sel = document.getElementById('slParentGroup');
    PopulateOptionSelect(sel, skill_parent_groups, null, 'All');
    sel.value = -1;

    var sel = document.getElementById('slGroup');
    PopulateOptionSelect(sel, skill_groups, null, 'All');
    sel.value = -1;

    sel = document.getElementById('slMinLevel');
    PopulateOptionSelect(sel, skill_types, null, null);
    sel.value = 0;

    SelectParentGroup(-1);
}

// Add skills to skill dropdowns for search 
function PopulateSkillOptions() {
    var skills = RetrieveSkills();

    var sel = document.getElementById('slSkillSearch1');
    PopulateOptionSelect(sel, skills, null, "None")

    var sel = document.getElementById('slSkillSearch2');
    PopulateOptionSelect(sel, skills, null, "Any")

    var sel = document.getElementById('slSkillSearch3');
    PopulateOptionSelect(sel, skills, null, "Any")

    SelectSkillSearch();
}

// Add people to people drop down `
function PopulatePeopleOptions() {
    var manager = GetSelectedManager();
    var location = GetSelectedLocation();

    var people = RetrievePeople(manager, location);

    var sel = document.getElementById('slPeople');

    PopulateOptionSelect(sel, people, null, "None" )

    SelectPerson(-1);
}

// Handler for upload file button
$(document).ready(function(){

    $(".flex-container").hide()
    $("input[type=file]").on('change',function(){

        // Get path to file and trim off all but the file name
        var fullPath = document.getElementById('fileUpload').value;
        if (fullPath) {
            var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
            var filename = fullPath.substring(startIndex);
            if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                filename = filename.substring(1);
            }
            var button = document.getElementById("upload-label");
            button.innerHTML = filename;
        }

        Upload();
    });
});

// Action on user selecting file to upload.
function Upload() {
    var fileUpload = document.getElementById("fileUpload");
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.csv|.txt)$/;
    if (regex.test(fileUpload.value.toLowerCase())) {
        if (typeof (FileReader) != "undefined") {

            $(".flex-container").show()

            var reader = new FileReader();
            reader.onload = function (e) {
                var rows = e.target.result.split("\n");

                PopulateStore(rows);
                PopulateManagerOptions();
                PopulateLocationOptions(-1);
                PopulatePeopleOptions();
                PopulateSkillOptions();
                PopulateParentGroupSkillOptions();
            }
            reader.readAsText(fileUpload.files[0]);
        } else {
            alert("This browser does not support HTML5.");
        }
    } else {
        alert("Please upload a valid CSV file.");
    }
}

// Handle user clicking on a Doughnut segment
function DoughnutClickEvent(evt, item) {
    if (item[0]) {
        clickedSkillName = item[0]._model.label;

        if (clickedSkillName == "Others") {
            PopulateDoughnutChart(GetSelectedParentGroup(), GetSelectedGroup(), GetSelectedMinLevel(), window.other_offset + window.max_skills);
        } else if (clickedSkillName == "Go Back") {
            PopulateDoughnutChart(GetSelectedParentGroup(), GetSelectedGroup(), GetSelectedMinLevel(), window.other_offset - window.max_skills);
        }
        else {
            GotoSkillSearch(FindSkillIndexByName(clickedSkillName), 0);
        }
    }
}

// Return a colour gradient num_colours long, from black to bright green, via red and blue.
function GetColourGradient(num_colours) {

    // Get a sorted array of the gradient keys
    var gradientKeys = Object.keys(window.gradient);
    
    gradientKeys.sort(function(a, b) {
            return +a - +b;
        });

    // Compute the gradient
    var colourGradient = [];
    for (var i = 0; i < num_colours; i++) {
        var gradientIndex = (i + 1) * (100 / (num_colours + 1)); //Find where to get a color from the gradient
        for (var j = 0; j < gradientKeys.length; j++) {
            var gradientKey = gradientKeys[j];
            if (gradientIndex === +gradientKey) { //Exact match with a gradient key - just get that color
                colourGradient[i] = 'rgba(' + gradient[gradientKey].toString() + ')';
                break;
            } else if (gradientIndex < +gradientKey) { //It's somewhere between this gradient key and the previous
                var prevKey = gradientKeys[j - 1];
                var gradientPartIndex = (gradientIndex - prevKey) / (gradientKey - prevKey); //Calculate where
                var color = [];
                for (var k = 0; k < 4; k++) { //Loop through Red, Green, Blue and Alpha and calculate the correct color and opacity
                    color[k] = gradient[prevKey][k] - ((gradient[prevKey][k] - gradient[gradientKey][k]) * gradientPartIndex);
                    if (k < 3) color[k] = Math.round(color[k]);
                }
                colourGradient[i] = 'rgba(' + color.toString() + ')';
                break;
            }
        }
    }

    return colourGradient;
}

// Function to action the Doughnut chart options.
function doughnutMenu(priority) {
    var $menu = $('#contextMenu');
    var item = myDoughnut.getElementsAtEvent( window.doughnut_event );

    $menu.hide()

    if (item[0]) {
        clickedSkillName = item[0]._model.label;
        skill_index = FindSkillIndexByName(clickedSkillName);

        GotoSkillSearch(skill_index, priority)
    }
}

// Retrieve the additional information for the Skill tooltip
function RetrieveSkillTips(skill_name) {

    var skill_index = FindSkillIndexByName(skill_name);

    var level_totals = [ 
            "Group: "+window.all_skills[skill_index].group,
            "Parent: "+window.all_skills[skill_index].parent, 
            "Breakdown"
        ];

    for (var j = window.skill_types[0].length; j>=window.min_level; j--) {
        total = window.all_skills[skill_index].levelTotal[j];
        if (total > 0) {
            type = window.all_skills[skill_index].type
            level_totals.push("  " + window.skill_types[type][j-1]+": "+String(total));
        }
    }

    return level_totals;
}

// Create or populate the skills Doughnut chart for all people
function PopulateDoughnutChart(parent, group, min_level, other_offset) {
    
    var skill_list = RetrieveSkillTotals(parent, group, min_level);

    window.min_level = min_level;
    window.other_offset = other_offset

    // If we have clicked Others then remove previously displayed items and add Go Back entry
    if (other_offset > 0)
    {
        var end = skill_list.length - other_offset;
        skill_list.splice(end, other_offset, { name: 'Go Back', total: skill_list[end].total})
    }

    // If we have too many items, truncate and add Other entry
    if (skill_list.length > window.max_skills) {
        var other_skill_list = skill_list.splice(0, skill_list.length - window.max_skills, { name: 'Others', total: 0});
        skill_list[0].total = other_skill_list[other_skill_list.length-1].total;
    }
    else {
        var other_skill_list = [];
    }

    var skill_names = [];
    var skill_totals = [];
    for (var i=0; i<skill_list.length; i++) {
        skill_names.push(skill_list[i].name);
        skill_totals.push(skill_list[i].total);
    }

    // Set title for chart
    if (group == 'All') {
        title = parent + " Skills " + (window.other_offset > 0 ? "+" +String(window.other_offset)+" more" : "")
    } else {
        title = group + " Skills " + (window.other_offset > 0 ? "+" +String(window.other_offset)+" more" : "")
    }

    var colourGradient = GetColourGradient(skill_list.length);

    if (!window.myDoughnut) {
        var canvas = document.getElementById('canvasDN');
     
        var config = {
            type: 'doughnut',
            data: {
                datasets: [{
                    data: skill_totals,
                    backgroundColor: colourGradient
                }],
                labels: skill_names
            },
            options: {
                responsive: true,
                onClick: DoughnutClickEvent,
                title: {
                    display: true,
                    text: title,
                    fontSize: 16
                },
                legend: {
                    display: false
                },
                animation: {
                    animateScale: true,
                    animateRotate: true
                },
                tooltips: {
                    callbacks: {
                        afterLabel: function(tooltipItem, data) {
                            afterLabels = [];

                            if (tooltipItem.index >= 0) {
                                name = data.labels[tooltipItem.index];

                                if (name == 'Others') {
                                    afterLabels.push(String(other_skill_list.length)+" more skills")
                                } else if (name == 'Go Back') {
                                    afterLabels.push(String(window.other_offset)+" skipped skills")
                                } else {
                                    afterLabels = RetrieveSkillTips(name)
                                }
                            }

                            return afterLabels;
                        }
                    }
                },
                plugins: {
                    labels: [
                        {
                            render: 'label',
                            fontColor: '#000',
                            position: 'outside'
                        },
                        {
                            render: 'value',
                            fontColor: '#FFF'
                        } ]
                }
            }
        };

        window.myDoughnut = new Chart(document.getElementById('canvasDN'), config);

        var $menu = $('#contextMenu');

        // Local function to handle right click on chart
        function handleContextMenu(e){
            var item = myDoughnut.getElementsAtEvent(e);
            if (item[0]) {
                e.preventDefault();
                e.stopPropagation();
                window.doughnut_event = e;
                $menu.css({left:(+e.pageX-30),top:(+e.pageY-25)});
                $menu.show();
                return(false);
            } else {
                return(true);
            }
        }

        // Local function to handle click off of menu area
        function handleMouseDown(e){
            $menu.hide();
        }

        canvas.addEventListener('contextmenu', handleContextMenu, false);
        canvas.addEventListener('mousedown', handleMouseDown, false);

    } else {
        window.myDoughnut.data.labels = skill_names;
        window.myDoughnut.data.datasets[0].data = skill_totals;
        window.myDoughnut.data.datasets[0].backgroundColor = colourGradient;
        window.myDoughnut.options.title.text = title

        window.myDoughnut.update();
    }

}

// Function to create and populate a table.  
function CreateTable(header, rows, id) {
    var table = document.createElement("table");

    if (id) { table.id = id; }

    var table_header = table.createTHead();
    var row = table_header.insertRow(-1);
    for (var j = 0; j < header.length; j++) {
        var cell = document.createElement("th");
        cell.innerHTML = header[j];
        row.appendChild(cell)
    }
    var body = table.createTBody();
    for (var i = 0; i < rows.length; i++) {
        var row = body.insertRow(-1);
        for (var j = 0; j < rows[i].length; j++) {
            var cell = row.insertCell(-1);
            cell.innerHTML = rows[i][j];
        }
    }

    return table;
}


// Function to highlight a table row, removing any other highlight
// select_row must be provided, to unselect all set select to false
function HighlightTableRow(select_row, select) {
    if (select_row) {
        var table = select_row.parentNode;
        for (var i = 0, row; row = table.rows[i]; i++) {
            if (select && row == select_row) {
                row.cells[0].style.color = 'purple';
            } else {
                row.cells[0].style.cssText = "";
            }
        }
    }
}

// Update search results table for selected skill
function PopulateSkillResults() {

    var selSkill = [];
    var skill_index = [];
    var skill_names = [];
    var i,j;

    selSkill.push(document.getElementById("slSkillSearch1"));
    selSkill.push(document.getElementById("slSkillSearch2"));
    selSkill.push(document.getElementById("slSkillSearch3"));

    for (i=0; i<selSkill.length; i++) {
        skill_index.push(selSkill[i].value);
        skill_names.push(skill_index[i] == -1 ? '' : window.all_skills[skill_index[i]].name);
    }

    var manager = GetSelectedManager();
    var location = GetSelectedLocation();

    var people_with_skill = RetrievePeopleWithSkill(manager, location, skill_names);

    var header = ["Full Name"];

    if (manager == 'Any') {
        header.push("Manager")
    }
    
    if (location == 'Any') {
        header.push("Location")
    }

    for (j = 0; j < skill_names.length; j++) {
        if (skill_names[j] != '') header.push(skill_names[j]);
    }

    var rows = []
    for (i=0; i<people_with_skill.length; i++) {
        var row = [people_with_skill[i].name];
        if (manager == 'Any') {
            row.push(people_with_skill[i].manager)
        }
        if (location == 'Any') {
            row.push(people_with_skill[i].location)
        }

        for (j = 0; j < skill_names.length; j++) {
            if (skill_names[j] != '') row.push(people_with_skill[i].held[j]);
        }
        rows.push(row);
    }

    var dvSkillSearch = document.getElementById("dvSkillResults");
    if (dvSkillSearch) {
        dvSkillSearch.innerHTML = "";

        if (people_with_skill.length > 0) {
            var table = CreateTable(header, rows, "tbSkillResults");

            // Local function to create function to handle click on row
            var createClickHandler = function(row) {
                    return function() {
                        var cell = row.getElementsByTagName("td")[0];
                        var name = cell.innerHTML;

                        var person_index = -1;
                        
                        selPerson = document.getElementById("slPeople");
                        for (var k = 0; k < selPerson.options.length; k++) {
                            if (selPerson.options[k].innerHTML == name) {
                                person_index = Number(selPerson.options[k].value);
                                break;
                            }
                        }
                        if (person_index != -1) {
                            selPerson.value = person_index;
                            SelectPerson(person_index)
                            document.getElementById("h1Person").scrollIntoView();
                        }
                    };
                };

            for (i=0; i < table.tBodies[0].rows.length; i++) {
                var row = table.tBodies[0].rows[i];
                row.cells[0].className = 'as-link';
                row.cells[0].onclick = createClickHandler(row);
            }

            dvSkillSearch.appendChild(table);
        }
        
        document.getElementById("slPeople").value = -1;
        SelectPerson(-1);
        
    }
}

// Function to handle a change of person
function SelectPerson(index) {
    if (window.people.length > index) {
        var selectedPerson = GetSelectedPerson();

        if (selectedPerson)
        {
            var tbSkillResults = document.getElementById("tbSkillResults");
            if  (tbSkillResults) {
                var tbody = tbSkillResults.tBodies[0];
                var found = false;
                for (var i = 0, row; row = tbody.rows[i]; i++) {
                    if (row.cells[0].innerHTML == selectedPerson.name) {
                        HighlightTableRow(row, true);
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    HighlightTableRow(tbody.rows[0], false);
                }
            }

            PopulatePerson(selectedPerson);
            PopulateSkills(selectedPerson.skills);
            PopulateRadarChart(selectedPerson);
        } else {
            PopulatePerson(null);
            PopulateSkills(null);
            PopulateRadarChart(null);
        }
    }
}

// Function to convert a parent index to a name
function GetParentGroupFromIndex(parent_index) {
    return (parent_index == -1 ? 'All' : window.all_parents[parent_index]);
}

// Function to convert a group index to a name
function GetGroupFromIndex(group_index) {
    if (window.selected_groups.length == 0) {
        return (group_index == -1 ? 'All' : window.all_groups[group_index]);
    } else {
        return (group_index == -1 ? 'All' : window.selected_groups[group_index]);
    }
}

// Get currently selected manager
function GetSelectedManager() {

    var selManager = document.getElementById('slManager');

    return selManager.options[selManager.selectedIndex].text;
}

// Get currently selected manager
function GetSelectedLocation() {

    var selLocation = document.getElementById('slLocation');

    return selLocation.options[selLocation.selectedIndex].text;
}
// Get currently selected parent group
function GetSelectedParentGroup() {

    var selParentGroup = document.getElementById('slParentGroup');

    return selParentGroup.options[selParentGroup.selectedIndex].text;
}

// Get currently selected group
function GetSelectedGroup() {
    
    var selGroup = document.getElementById('slGroup');

    return selGroup.options[selGroup.selectedIndex].text;
}

// Get currently selected minimum level
function GetSelectedMinLevel() {
    
    var selMinLevel = document.getElementById('slMinLevel');

    return Number(selMinLevel.value);
}

// Get currently selected minimum level
function GetSelectedPerson() {
    
    var selPeople = document.getElementById('slPeople');

    var name = selPeople.options[selPeople.selectedIndex].text;

    var index = FindPropertyIndex(window.people, "name", name);

    return window.people[index];
}

// Build limited list of groups based on selected parent group
function BuildSelectedGroups(selectedParent) {

    window.selected_groups = [];

    if (selectedParent != 'All') {
        for (var i = 0; i < window.all_skills.length; i++) {
            if (window.all_skills[i].parent == selectedParent) {
                var found = false;
                for (var j = 0; j < window.selected_groups.length; j++) {
                    if (window.selected_groups[j] == window.all_skills[i].group) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    window.selected_groups.push(window.all_skills[i].group);
                }
            }
        }
        window.selected_groups.sort();
    }
}

// Function called when a manager is selected
function SelectManager(index) {
    if (window.managers.length > index) {

        PopulateLocationOptions(index);

        PopulateSkillsStore(GetSelectedManager(), GetSelectedLocation())

        PopulatePeopleOptions();
        PopulateSkillOptions();
        PopulateParentGroupSkillOptions();
    }
}

// Function called when a location is selected
function SelectLocation(index) {
    if (window.locations.length > index) {
        PopulateSkillsStore(GetSelectedManager(), GetSelectedLocation())

        PopulatePeopleOptions();
        PopulateSkillOptions();
        PopulateParentGroupSkillOptions();
    }
}

// Function to handle a change of parent group
function SelectParentGroup(index) {
    if (window.all_parents.length > index) {

        var selectedParent = GetSelectedParentGroup();

        BuildSelectedGroups(selectedParent)

        var sel = document.getElementById('slGroup');
        if (selectedParent == 'All') {
            PopulateOptionSelect(sel, window.all_groups, null, 'All');
        } else {
            PopulateOptionSelect(sel, window.selected_groups, null, 'All');
        }
        sel.value = -1;

        PopulateDoughnutChart(selectedParent, GetSelectedGroup(), GetSelectedMinLevel(), 0);
    }
}

// Function to handle a change of groupGetSelectedGroup
function SelectGroup(index) {
    if (window.all_groups.length > index) {

        var selectedGroup = GetSelectedGroup();
        var parentGroup = GetSelectedParentGroup();
        
        for (var i = 0; i < window.all_skills.length; i++) {
            if (window.all_skills[i].group == selectedGroup) {
                parentGroup = window.all_skills[i].parent;
                break;
            }
        }

        document.getElementById('slParentGroup').value = window.all_parents.indexOf(parentGroup);

        PopulateDoughnutChart(parentGroup, selectedGroup, GetSelectedMinLevel(), 0);
    }
}

// Function to handle a change of minimum level
function SelectMinLevel(min_level) {
    if (window.skill_types[0].length > min_level) {
        PopulateDoughnutChart(GetSelectedParentGroup(), GetSelectedGroup(), Number(min_level), 0);
    }
}

// Function to reset doughnut selections 
function ResetDoughnut() {
    window.selected_groups = [];

    document.getElementById('slParentGroup').value = -1;
    document.getElementById('slGroup').value = -1;
    document.getElementById('slMinLevel').value = 0;

    SelectParentGroup(-1);
}

// Function to handle a change of skill for the skill search
function SelectSkillSearch() {
    if (window.all_skills.length > 0) {
        PopulateSkillResults();
    }
}

function ResetSearch() {
    document.getElementById("slSkillSearch1").value = -1;
    document.getElementById("slSkillSearch2").value = -1;
    document.getElementById("slSkillSearch3").value = -1;

    PopulateSkillResults();
}

// Populate HTML table with person details
function PopulatePerson(thisPerson) {
    var dvPerson = document.getElementById("dvPerson");
    if (dvPerson) {
        dvPerson.innerHTML = "";
    
        if (thisPerson) {
            header = ["Full Name", "Staff No", "Email", "Manager", 'Location'];
            rows = [[thisPerson.name, thisPerson.no, thisPerson.email, thisPerson.manager, thisPerson.location]];

            dvPerson.appendChild(CreateTable(header, rows, "tbPerson"));
        }
    }
}

// Populate HTML table with person skill details
function PopulateSkills(skills) {
    var dvSkills = document.getElementById("dvSkills");
    if (dvSkills) {
        dvSkills.innerHTML = "";

        if (skills) {
            header = [ "Skill", "Required", "Held"  ];
            rows = []
            for (var j = 0; j < skills.length; j++) {
                rows.push([ skills[j].name,  skills[j].required, skills[j].held ])
            }

            dvSkills.appendChild(CreateTable(header, rows, "tbSKills"));
        }
    }
}

// Function to populate the skill search criteria, populate the table and scroll to it.
function GotoSkillSearch(skill_index, priority) {
    if (skill_index != -1) {
        var selSkill1 = document.getElementById('slSkillSearch1');
        var selSkill2 = document.getElementById("slSkillSearch2");
        var selSkill3 = document.getElementById("slSkillSearch3");
        if (priority <= 1) {
            selSkill1.value = skill_index;
        }
        
        if (priority == 2) {
            selSkill2.value = skill_index;
        } else if (priority == 0)  {
            selSkill2.value = -1;
        }
        
        if (priority == 3) {
            selSkill3.value = skill_index;
        } else if (priority == 0) {
            selSkill3.value = -1;
        }

        PopulateSkillResults();

        document.getElementById("h1Search").scrollIntoView();
    }         
}

// Populate radar chart of persons skills
function PopulateRadarChart(selectedPerson) {
    var color = Chart.helpers.color;

    var skill_names = [];
    var skill_levels = [];
    var skill_required = [];
    var title = "None"

    if (selectedPerson) {
        for (var i = 0; i < selectedPerson.skills.length; i++) {
            skill_names.push(selectedPerson.skills[i].name);
            skill_levels.push(selectedPerson.skills[i].heldLevel);
            skill_required.push(selectedPerson.skills[i].requiredLevel);
        }
        window.currentPerson = selectedPerson;
        title = selectedPerson.name;
    }

    skill_names.reverse();
    skill_levels.reverse();
    skill_required.reverse();

    if (!window.myRadar) {
        var config = {
            type: 'radar',
            data: {
                labels: skill_names,
                datasets: [{
                    label: "Held Skills",
                    backgroundColor: color(window.chartColors.green).alpha(0.2).rgbString(),
                    borderColor: window.chartColors.green,
                    pointBackgroundColor: window.chartColors.green,
                    data: skill_levels
                }, {
                    label: "Required Skills",
                    backgroundColor: color(window.chartColors.red).alpha(0.2).rgbString(),
                    borderColor: window.chartColors.red,
                    pointBackgroundColor: window.chartColors.red,
                    data: skill_required,
                    borderDash: [10, 5]
                }]
            },
            options: {
                responsive: true,
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: title,
                    fontSize: 16
                },
                scale: {
                    ticks: {
                        beginAtZero: true,
                        max: 5,
                        min: 0,
                        stepSize: 1,
                        display: false
                    }
                },
                tooltips: {
                    callbacks: {
                        title: function (tooltipItem, data) {
                            return data.labels[tooltipItem[0].index];
                        },
                        label: function (tooltipItem, data) {
                            dataset = tooltipItem.datasetIndex;
                            item = tooltipItem.index;
                            tip_title = data.labels[item];
                            tip_level = data.datasets[dataset].data[item];
                            tip_label = String(tip_level)
                            for (var k = 0; k < window.currentPerson.skills.length; k++) {
                                if (window.currentPerson.skills[k].name == tip_title) {
                                    held_level = ConvertToLevel(window.currentPerson.skills[k].held);
                                    if (held_level == tip_level) {
                                        tip_label = window.currentPerson.skills[k].held;
                                    } else {
                                        tip_label = window.currentPerson.skills[k].required;
                                    }
                                }
                            }
                            
                            return tip_label;
                        }
                    }
                }
            }
        };

        window.myRadar = new Chart(document.getElementById('canvasRP'), config);
    } else {
        window.myRadar.data.labels = skill_names;
        window.myRadar.data.datasets[0].data = skill_levels;
        window.myRadar.data.datasets[1].data = skill_required;
        window.myRadar.options.title.text = title;

        window.myRadar.update()
    }
}