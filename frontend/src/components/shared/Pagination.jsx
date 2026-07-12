// src/components/shared/Pagination.jsx
import { ChevronLeft, ChevronRight } from "lucide-react";
import { Button } from "../ui/button";

// eslint-disable-next-line react/prop-types
const Pagination = ({ currentPage, totalPages, onPageChange }) => {

    if (totalPages <= 1) return null; // don't show if only 1 page

    // Build page numbers to show
    const getPageNumbers = () => {
        const pages = [];

        if (totalPages <= 7) {
            // show all pages
            for (let i = 1; i <= totalPages; i++) {
                pages.push(i);
            }
        } else {
            // show first, last, current and neighbours
            pages.push(1);

            if (currentPage > 3) pages.push("...");

            for (let i = Math.max(2, currentPage - 1);
                i <= Math.min(totalPages - 1, currentPage + 1);
                i++) {
                pages.push(i);
            }

            if (currentPage < totalPages - 2) pages.push("...");

            pages.push(totalPages);
        }

        return pages;
    };

    return (
        <div className="flex items-center justify-center gap-2 mt-8">

            {/* Previous Button */}
            <Button
                variant="outline"
                size="sm"
                onClick={() => {
                    console.log("prev CLICKED");
                    onPageChange(currentPage - 1);
                }
                }
                disabled={currentPage === 1}
                className="flex items-center gap-1">
                <ChevronLeft className="h-4 w-4" />
                Previous
            </Button>

            {/* Page Numbers */}
            {getPageNumbers().map((page, index) => (
                page === "..." ? (
                    <span key={index} className="px-2 text-gray-400">
                        ...
                    </span>
                ) : (
                    <Button
                        key={index}
                        variant={currentPage === page ? "default" : "outline"}
                        size="sm"
                        onClick={() =>{ 
                            console.log(`Page ${page} clicked`);
                            onPageChange(page)}}
                        className={`w-9 h-9 ${currentPage === page
                                ? "bg-blue-600 text-white"
                                : "text-gray-600"
                            }`}>
                        {page}
                    </Button>
                )
            ))}

            {/* Next Button */}
            <Button
                variant="outline"
                size="sm"
                onClick={() => {
                    console.log("next CLICKED");
                    onPageChange(currentPage + 1)
                }}
                disabled={currentPage === totalPages}
                className="flex items-center gap-1">
                Next
                <ChevronRight className="h-4 w-4" />
            </Button>
        </div>
    );
};

export default Pagination;