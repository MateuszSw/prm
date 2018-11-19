export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class ResultPaginate<T> {
    score: T;
    pagination: Pagination;
}
