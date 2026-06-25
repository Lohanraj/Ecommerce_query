



const MAX_ROWS = 12;   


const STATUS_CYCLE = ['ON TIME', 'BOARDING', 'GATE CLOSED', 'DEPARTED'];


const STATUS_CLASS = {
  'ON TIME':    'on-time',
  'BOARDING':   'boarding',
  'GATE CLOSED':'gate-closed',
  'DEPARTED':   'departed',
  'DELAYED':    'delayed',
  'CANCELLED':  'cancelled',
};


const STATUS_DOT_COLOR = {
  'ON TIME':    'var(--green)',
  'BOARDING':   '#26d07c',
  'GATE CLOSED':'var(--gold)',
  'DEPARTED':   'var(--dim)',
  'DELAYED':    'var(--red)',
  'CANCELLED':  'var(--red)',
};




let nextId = 1;

function makeId() { return nextId++; }


const INITIAL_FLIGHTS = [
  { time: '06:15', flight: 'LH 401',  dest: 'Frankfurt',    gate: 'A3',  status: 'DEPARTED'   },
  { time: '07:30', flight: 'BA 302',  dest: 'London Heathrow', gate: 'B7',  status: 'DEPARTED' },
  { time: '08:45', flight: 'AF 118',  dest: 'Paris CDG',    gate: 'C2',  status: 'GATE CLOSED' },
  { time: '09:20', flight: 'TK 892',  dest: 'Istanbul',     gate: 'D11', status: 'BOARDING'   },
  { time: '10:05', flight: 'EK 055',  dest: 'Dubai',        gate: 'A9',  status: 'BOARDING'   },
  { time: '11:00', flight: 'AA 204',  dest: 'New York JFK', gate: 'E2',  status: 'ON TIME'    },
  { time: '12:30', flight: 'QR 074',  dest: 'Doha',         gate: 'F4',  status: 'ON TIME'    },
  { time: '13:10', flight: 'SQ 317',  dest: 'Singapore',    gate: 'B12', status: 'DELAYED'    },
  { time: '14:50', flight: 'UA 880',  dest: 'Chicago O\'Hare', gate: 'C7', status: 'ON TIME'  },
  { time: '16:25', flight: 'NH 204',  dest: 'Tokyo Narita', gate: 'A15', status: 'ON TIME'    },
];


let flights = [];


const EXTRA_FLIGHTS = [
  { time: '17:40', flight: 'CX 231',  dest: 'Hong Kong',    gate: 'D3',  status: 'ON TIME' },
  { time: '18:15', flight: 'KL 883',  dest: 'Amsterdam',    gate: 'B2',  status: 'ON TIME' },
  { time: '19:30', flight: 'MH 130',  dest: 'Kuala Lumpur', gate: 'E9',  status: 'ON TIME' },
  { time: '20:05', flight: 'OS 551',  dest: 'Vienna',       gate: 'A6',  status: 'ON TIME' },
  { time: '21:50', flight: 'AC 854',  dest: 'Toronto',      gate: 'C10', status: 'ON TIME' },
  { time: '22:10', flight: 'JL 043',  dest: 'Tokyo Haneda', gate: 'F3',  status: 'ON TIME' },
  { time: '23:00', flight: 'DL 402',  dest: 'Atlanta',      gate: 'B5',  status: 'ON TIME' },
];
let extraIndex = 0;  




const boardEl      = document.getElementById('board');
const clockEl      = document.getElementById('clock');
const dateEl       = document.getElementById('date');
const summaryEl    = document.getElementById('summary-bar');
const btnAdd       = document.getElementById('btn-add');
const btnReset     = document.getElementById('btn-reset');
const btnCustom    = document.getElementById('btn-custom-toggle');
const customForm   = document.getElementById('custom-form');
const btnSubmit    = document.getElementById('btn-submit-custom');




function tickClock() {
  const now = new Date();

  
  const h = String(now.getHours()).padStart(2, '0');
  const m = String(now.getMinutes()).padStart(2, '0');
  const s = String(now.getSeconds()).padStart(2, '0');

  
  clockEl.textContent = `${h}:${m}:${s}`;

  
  const days   = ['SUN','MON','TUE','WED','THU','FRI','SAT'];
  const months = ['JAN','FEB','MAR','APR','MAY','JUN',
                  'JUL','AUG','SEP','OCT','NOV','DEC'];
  dateEl.textContent =
    `${days[now.getDay()]} ${now.getDate()} ${months[now.getMonth()]} ${now.getFullYear()}`;
}


tickClock();
setInterval(tickClock, 1000);




function renderSummary() {
  
  const total    = flights.length;
  const boarding = flights.filter(f => f.status === 'BOARDING').length;
  const delayed  = flights.filter(f => f.status === 'DELAYED').length;
  const departed = flights.filter(f => f.status === 'DEPARTED').length;

  
  while (summaryEl.firstChild) summaryEl.removeChild(summaryEl.firstChild);

  
  function makePill(label, count, color) {
    if (count === 0) return;               

    
    const pill = document.createElement('div');
    pill.className = 'pill';

    
    const dot = document.createElement('span');
    dot.className = 'pill-dot';
    dot.style.background = color;

    
    const txt = document.createElement('span');
    txt.textContent = `${count} ${label}`;

    
    pill.appendChild(dot);
    pill.appendChild(txt);
    summaryEl.appendChild(pill);
  }

  makePill('departures',  total,    'var(--dim)');
  makePill('boarding',    boarding, 'var(--green)');
  makePill('delayed',     delayed,  'var(--red)');
  makePill('departed',    departed, 'var(--dim)');
}




function showEmptyState() {
  
  const empty = document.createElement('div');
  empty.className = 'empty-state';

  
  const icon = document.createElement('span');
  icon.className = 'empty-icon';
  icon.textContent = '✈';

  
  const msg = document.createElement('p');
  msg.textContent = 'No departures scheduled';

  
  empty.appendChild(icon);
  empty.appendChild(msg);
  boardEl.appendChild(empty);
}




function buildRow(flight) {
  
  const row = document.createElement('div');
  row.className = 'flight-row';

  
  row.dataset.id = flight.id;

  
  function makeCell(text, extraClass) {
    const cell = document.createElement('span');
    cell.className = `cell ${extraClass}`;
    cell.textContent = text;  
    return cell;
  }

  
  row.appendChild(makeCell(flight.time,   'cell-time'));
  row.appendChild(makeCell(flight.flight, 'cell-flight'));
  row.appendChild(makeCell(flight.dest,   'cell-dest'));
  row.appendChild(makeCell(flight.gate,   'cell-gate'));

  
  row.appendChild(buildStatusBadge(flight.status));

  return row;
}


function buildStatusBadge(status) {
  const badge = document.createElement('span');
  badge.className = `cell cell-status ${STATUS_CLASS[status] || 'on-time'}`;
  badge.textContent = status;
  return badge;
}




function renderBoard() {
  
  while (boardEl.firstChild) boardEl.removeChild(boardEl.firstChild);

  if (flights.length === 0) {
    showEmptyState();
  } else {
    
    for (const flight of flights) {
      const row = buildRow(flight);
      boardEl.appendChild(row);   
    }
  }

  renderSummary();
}




function appendRow(flightData) {
  
  if (flights.length >= MAX_ROWS) {
    flights.shift();          
    const oldestRow = boardEl.querySelector('.flight-row');
    if (oldestRow) boardEl.removeChild(oldestRow);  
  }

  
  const emptyEl = boardEl.querySelector('.empty-state');
  if (emptyEl) boardEl.removeChild(emptyEl);

  
  flights.push(flightData);

  
  const row = buildRow(flightData);
  boardEl.appendChild(row);

  
  sortAndRerender();
}


function sortAndRerender() {
  
  flights.sort((a, b) => a.time.localeCompare(b.time));
  renderBoard();
}




function cycleOneStatus() {
  if (flights.length === 0) return;

  
  const eligible = flights.filter(f =>
    f.status !== 'DEPARTED' && f.status !== 'CANCELLED'
  );
  if (eligible.length === 0) return;

  const flight = eligible[Math.floor(Math.random() * eligible.length)];

  
  const idx     = STATUS_CYCLE.indexOf(flight.status);
  const nextIdx = idx === -1 ? 1 : Math.min(idx + 1, STATUS_CYCLE.length - 1);
  if (nextIdx === idx) return; 

  
  flight.status = STATUS_CYCLE[nextIdx];

  
  const row = boardEl.querySelector(`.flight-row[data-id="${flight.id}"]`);
  if (!row) return;

  
  const badge = row.querySelector('.cell-status');
  if (!badge) return;

  

  
  for (const cls of Object.values(STATUS_CLASS)) badge.classList.remove(cls);

  
  badge.classList.add('flip');
  badge.addEventListener('animationend', () => {
    badge.classList.remove('flip');
    badge.classList.add(STATUS_CLASS[flight.status] || 'on-time');
    badge.textContent = flight.status;  
  }, { once: true });

  
  renderSummary();
}


setInterval(cycleOneStatus, 4000);




btnAdd.addEventListener('click', () => {
  
  const template = EXTRA_FLIGHTS[extraIndex % EXTRA_FLIGHTS.length];
  extraIndex++;

  
  const newFlight = { ...template, id: makeId() };

  appendRow(newFlight);
});




btnReset.addEventListener('click', () => {
  extraIndex = 0;

  
  flights = INITIAL_FLIGHTS.map(f => ({ ...f, id: makeId() }));

  renderBoard();

  
  customForm.hidden = true;
});




btnCustom.addEventListener('click', () => {
  customForm.hidden = !customForm.hidden;
  btnCustom.textContent = customForm.hidden ? '✏ Custom Flight' : '✕ Close Form';
});




btnSubmit.addEventListener('click', () => {
  
  const time   = document.getElementById('f-time').value.trim()   || '00:00';
  const flight = document.getElementById('f-flight').value.trim() || 'XX 000';
  const dest   = document.getElementById('f-dest').value.trim()   || 'Unknown';
  const gate   = document.getElementById('f-gate').value.trim()   || '--';

  const newFlight = {
    id:     makeId(),
    time,
    flight: flight.toUpperCase(),
    dest,
    gate:   gate.toUpperCase(),
    status: 'ON TIME',
  };

  appendRow(newFlight);

  
  ['f-time','f-flight','f-dest','f-gate'].forEach(id => {
    document.getElementById(id).value = '';
  });

  
  customForm.hidden = true;
  btnCustom.textContent = '✏ Custom Flight';
});





flights = INITIAL_FLIGHTS.map(f => ({ ...f, id: makeId() }));


renderBoard();

