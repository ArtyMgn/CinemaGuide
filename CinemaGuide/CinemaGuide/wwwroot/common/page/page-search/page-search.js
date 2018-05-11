const BASE_URL = 'http://localhost:9201/';

async function search(searchConfig, sourceList) {
    const url = new URL(`${BASE_URL}partial/search`);

    searchConfig.Query = document.querySelector('.search__input').value;

    for (let sourceName of sourceList) {
        const searchList = document.querySelector(`.search-list[source=${sourceName}]`);
        searchList.innerHTML = '<div class="loader"></div>';
        url.search = new URLSearchParams({ ...searchConfig, sourceName });
        const resultHtml = await fetch(url).then(res => res.text());
        searchList.innerHTML = resultHtml;
    }
}
