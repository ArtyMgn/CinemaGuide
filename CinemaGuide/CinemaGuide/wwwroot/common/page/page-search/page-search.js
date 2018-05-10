const BASE_URL = 'http://localhost:9201/';
let SEARCH_CONFIG = undefined;

function setFirstSource() {
    const switchRadio = document.querySelector('.switcher__radio');
    switchRadio.checked = true;
    switchSource(switchRadio.id);
}

async function search(query) {
    const result        = document.querySelector('.search-result');
    result.innerHTML    = '<div class="loader"></div>';
    const url           = new URL(`${BASE_URL}partial/search`);
    SEARCH_CONFIG.Query = query;
    url.search          = new URLSearchParams(SEARCH_CONFIG);
    const resultHTML    = await fetch(url).then(res => res.text());
    result.innerHTML = resultHTML;
    setFirstSource();
}
