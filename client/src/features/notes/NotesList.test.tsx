import { fireEvent, render, screen } from "@testing-library/react";
import NotesList from "./NotesList";
import type { Note } from "../../lib/types";
import { MemoryRouter, useNavigate } from "react-router-dom";

jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn()
}));

describe("NotesList", () => {
  const mockNavigate = jest.fn();

  beforeEach(() => {
    (useNavigate as jest.Mock).mockReturnValue(mockNavigate);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  const mockNotes: Note[] = [
    {
      id: 1,
      title: "Note 1",
      content: "Content 1",
      createdAt: new Date().toISOString()
    },
    {
      id: 2,
      title: "Note 2",
      content: "Content 2",
      createdAt: new Date().toISOString()
    }
  ];

  it("renders all notes", () => {
    render(<NotesList notes={mockNotes} />, { wrapper: MemoryRouter });

    mockNotes.forEach((note) => {
      expect(screen.getByTestId(`note-item-${note.id}`)).toBeDefined();
    });
  });

  it("navigates to /create on FAB click", () => {
    render(<NotesList notes={mockNotes} />, { wrapper: MemoryRouter });

    fireEvent.click(screen.getByRole("button", { name: /add/i }));

    expect(mockNavigate).toHaveBeenCalledWith("/create");
  });

  it("navigates to edit page on note click", () => {
    render(<NotesList notes={mockNotes} />, { wrapper: MemoryRouter });

    fireEvent.click(screen.getByTestId("note-item-2"));

    expect(mockNavigate).toHaveBeenCalledWith("/2/edit");
  });
});
