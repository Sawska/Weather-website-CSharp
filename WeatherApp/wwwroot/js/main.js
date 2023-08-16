const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");
// getting new date, current year and month
let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();
// storing full name of all months in array
const months = ["January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"];

const location = document.querySelector("#Location").textContent

async function getWeatherData(month,year) {
    const response = await fetch(`https://localhost:7246/api/Weather/GetHistoricalData?location=${location}k&month=${month}&year=${year}`);
    const data = await response.json();
    return data;
}

const renderCalendar = async () => {
    let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(),
        lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(),
        lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(),
        lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate();

    let weatherData = await getWeatherData(currMonth,currYear);


    let weatherDataPrevios = await getWeatherData(currMonth+1,currYear)
    let liTag = "";
    for (let i = firstDayofMonth; i > 0; i--) { // creating li of previous month last days
        liTag += `<li class="inactive"<div><img class="imgClass" src="${weatherDataPrevios.forecast.forecastday[i].day.condition.icon}" /></div><div>${lastDateofLastMonth - i + 1}</div></li>`;
    }
    for (let i = 1; i <= weatherData.forecast.forecastday.length; i++) { // creating li of all days of current month
        // finding the weather data for 7 AM of the current day


        let isToday = i === date.getDate() && currMonth === new Date().getMonth()
            && currYear === new Date().getFullYear() ? "active" : "";
            console.log(weatherData.forecast.forecastday[i-1].day.condition.icon)
        
        
            
            liTag += `<li class="${isToday}"><div><img class="imgClass" src="${weatherData.forecast.forecastday[i-1].day.condition.icon}" /></div><div>${i}</div></li>`;

    }
    let weatherDataNext = await getWeatherData(currMonth+1,currYear)
    for (let i = lastDayofMonth; i < 6; i++) { // creating li of next month first days
        liTag += `<li class="inactive"><div><img class="imgClass" src="${weatherDataNext.forecast.forecastday[i].day.condition.icon}" /></div><div>${i - lastDayofMonth + 1}</div></li>`
    }
    currentDate.innerText = `${months[currMonth]} ${currYear}`;
    daysTag.innerHTML = liTag;
}


renderCalendar();


prevNextIcon.forEach(icon => { // getting prev and next icons
    icon.addEventListener("click", () => { // adding click event on both icons
        // if clicked icon is previous icon then decrement current month by 1 else increment it by 1
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
        if(currMonth < 0 || currMonth > 11) { // if current month is less than 0 or greater than 11
            // creating a new date of current year & month and pass it as date value
            date = new Date(currYear, currMonth, new Date().getDate());
            currYear = date.getFullYear(); // updating current year with new date year
            currMonth = date.getMonth(); // updating current month with new date month
        } else {
            date = new Date(); // pass the current date as date value
        }
        renderCalendar(); // calling renderCalendar function
    });
});