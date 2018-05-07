const BASE_URL = 'http://localhost:9201/';
let SEARCH_CONFIG = undefined;

async function search(query) {
    const list          = document.querySelector('.search-list');
    list.innerHTML      = '<div class="loader"></div>';
    const url           = new URL(`${BASE_URL}partial/search`);
    SEARCH_CONFIG.Query = query;
    url.search          = new URLSearchParams(SEARCH_CONFIG);
    const result        = await fetch(url).then(res => res.text());
    list.innerHTML      = result;
}
