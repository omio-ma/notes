import type { UseQueryResult } from "@tanstack/react-query";
import { render, screen } from "@testing-library/react";
import * as useNotesModule from "../../lib/hooks/notes/useNotes";
import type { Note } from "../../lib/types";
import HomePage from "./HomePage";

// Mocking MUI CircularProgress to avoid unnecessary rendering
jest.mock("@mui/material", () => ({
  ...jest.requireActual("@mui/material"),
  CircularProgress: () => <div data-testid="loader">Loading...</div>
}));

jest.mock("../../lib/hooks/notes/useNotes");
jest.mock("../../lib/agent");

describe("HomePage", () => {
  const useNotesMock = useNotesModule as jest.Mocked<typeof useNotesModule>;

  it("shows loading state", () => {
    useNotesMock.useNotes.mockReturnValue({
      data: undefined,
      isLoading: true,
      error: null
    } as unknown as UseQueryResult<Note[]>);

    render(<HomePage />);
    expect(screen.getByTestId("loader")).toBeDefined();
  });

  it("shows error message", () => {
    useNotesMock.useNotes.mockReturnValue({
      data: undefined,
      isLoading: false,
      error: new Error("Fetch failed")
    } as unknown as UseQueryResult<Note[]>);

    render(<HomePage />);
    expect(screen.getByText(/Failed to load notes/i)).toBeDefined();
  });

  it("renders list of notes on success", () => {
    const mockNotes: Note[] = [
      {
        id: 1,
        title: "Note A",
        content: "A content",
        createdAt: new Date().toISOString()
      },
      {
        id: 2,
        title: "Note B",
        content: "B content",
        createdAt: new Date().toISOString()
      }
    ];

    useNotesMock.useNotes.mockReturnValue({
      data: mockNotes,
      isLoading: false,
      error: null
    } as unknown as UseQueryResult<Note[]>);

    render(<HomePage />);
    expect(screen.getByText("My Notes")).toBeDefined();
    expect(screen.getByTestId("note-item-1")).toBeDefined();
    expect(screen.getByTestId("note-item-2")).toBeDefined();
  });
});
